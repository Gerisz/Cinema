using Cinema.Data.Models;
using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            var movieTitles = await _context.Movies
                .AsNoTracking()
                .Select(MovieDTO.Projection)
                .ToListAsync();

            if (movieTitles == null)
                return NotFound();

            return Ok(movieTitles);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostMovie(MovieDTO movieCreate)
        {
            var movie = new Movie(movieCreate);

            try
            {
                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction(nameof(GetMovies), new { id = movie.Id }, movie);
        }
    }
}
