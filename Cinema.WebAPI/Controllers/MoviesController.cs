using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables;
using Cinema.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ICinemaService _service;

        public MoviesController(ICinemaService service)
        {
            _service = service;
        }

        public async Task<ActionResult<Movie>> GetMovie(Int32 id)
        {
            var movie = _service.GetMovieByIdAsync(id);

            if (await movie == null)
                return NotFound();

            return Ok(movie);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostMovie(MovieCreate movieCreate)
        {
            var movie = _service.CreateMovieAsync(new Movie(movieCreate));

            if (await movie == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }
    }
}
