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
    public class MoviesController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public MoviesController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesAsync()
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .Select(MovieDTO.Projection)
                .ToListAsync();

            if (movies == null)
                return NotFound();

            return Ok(movies);
        }

        [HttpGet("{id}", Name = "GetMovie")]
        public async Task<ActionResult<MovieDTO>> GetMovieAsync(Int32 id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostMovieAsync(MovieDTO movieDTO)
        {
            var movie = new Movie(movieDTO);

            try
            {
                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieAsync(Int32 id, MovieDTO movieDTO)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);

                if (movie == null)
                    return NotFound();

                movie.Update(new Movie(movieDTO));
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
        public async Task<IActionResult> DeleteMovieAsync(Int32 id)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);

                if (movie == null)
                    return NotFound();

                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }
    }
}
