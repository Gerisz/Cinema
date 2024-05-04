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
        public async Task<ActionResult<ShowDetails>> GetShowAsync(Int32 id)
        {
            var show = _context.Shows.FindAsync(id);

            if (await show == null)
                return NotFound();

            return Ok(ShowDetails.Create((await show)!));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostShowAsync(ShowCreate showCreate)
        {
            var movie = _context.Movies.FindAsync(showCreate.MovieId);
            var hall = _context.Halls.FindAsync(showCreate.HallId);

            if (await movie == null)
                return BadRequest($"No movie with ID {showCreate.MovieId} has been found.");
            if (await hall == null)
                return BadRequest($"No hall with ID {showCreate.HallId} has been found.");

            var show = new Show(showCreate.Start, (await movie)!, (await hall)!);

            try
            {
                await _context.Shows.AddAsync(show);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(GetShowAsync), new { id = show.Id }, show);
        }
    }
}
