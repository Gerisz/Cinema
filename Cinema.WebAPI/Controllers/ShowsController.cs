using Cinema.Data.Models;
using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public ShowsController(CinemaDbContext service)
        {
            _context = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowDTO>>> GetShowsAsync()
        {
            var shows = await _context.Shows
                .AsNoTracking()
                .Select(ShowDTO.Projection)
                .ToListAsync();

            if (shows == null)
                return NotFound();

            return Ok(shows);
        }

        [HttpGet("{id}", Name = "GetShow")]
        public async Task<ActionResult<ShowDTO>> GetShowAsync(Int32 id)
        {
            var show = _context.Shows.FindAsync(id);

            if (await show == null)
                return NotFound();

            return Ok(ShowDTO.Create((await show)!));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostShowAsync(ShowDTO showDTO)
        {
            var movie = await _context.Movies.FindAsync(showDTO.Movie.Id);
            var hall = await _context.Halls.FindAsync(showDTO.Hall.Id);

            if (movie == null)
                return BadRequest($"No movie with ID {showDTO.Movie.Id} has been found.");
            if (hall == null)
                return BadRequest($"No hall with ID {showDTO.Hall.Id} has been found.");
            if (await CheckShowForConflicts(showDTO))
                return BadRequest($"There must be at least " +
                    $"15 minutes between two shows in the same hall!");

            var show = new Show(showDTO.Start, movie, hall);

            try
            {
                await _context.Shows.AddAsync(show);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetShow", new { id = show.Id }, ShowDTO.Create(show));
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShowAsync(Int32 id, ShowDTO showDTO)
        {
            try
            {
                var show = await _context.Shows.FindAsync(id);
                var movie = await _context.Movies.FindAsync(showDTO.Movie.Id);
                var hall = await _context.Halls.FindAsync(showDTO.Hall.Id);

                if (show == null)
                    return NotFound();
                if (movie == null)
                    return BadRequest($"No movie with ID {showDTO.Movie.Id} has been found.");
                if (hall == null)
                    return BadRequest($"No hall with ID {showDTO.Hall.Id} has been found.");
                if (await CheckShowForConflicts(showDTO))
                    return BadRequest($"There must be at least " +
                        $"15 minutes between two shows in the same hall!");

                if (hall.Id != showDTO.Id)
                    _context.Seats.RemoveRange(show.Seats);
                show.Update(new Show(showDTO.Start, movie, hall, show.Seats));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShowAsync(Int32 id)
        {
            try
            {
                var show = await _context.Shows.FindAsync(id);

                if (show == null)
                    return NotFound();

                _context.Shows.Remove(show);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }

        private async Task<Boolean> CheckShowForConflicts(ShowDTO show)
        {
            var movie = await _context.Movies.FindAsync(show.Movie.Id);

            try
            {

                return _context.Shows
                    .ToList()
                    .Any(s => s.Id != show.Id &&
                        (From(s) < show.Start && show.Start < To(s))
                            || (From(s) < show.Start + new TimeSpan(0, movie!.Length, 0)
                                && show.Start + new TimeSpan(0, movie!.Length, 0) < To(s))
                        && show.Hall.Id == s.HallId);
            }
            catch (Exception) { return false; }
        }

        private DateTime From(Show show)
        {
            return show.Start - new TimeSpan(0, 15, 0);
        }

        private DateTime To(Show show)
        {
            return show.Start + new TimeSpan(0, 15 + show.Movie.Length, 0);
        }
    }
}
