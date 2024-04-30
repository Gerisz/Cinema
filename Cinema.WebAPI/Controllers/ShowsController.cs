using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables;
using Cinema.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly ICinemaService _service;

        public ShowsController(ICinemaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<Show>> GetShow(Int32 id)
        {
            var show = await _service.GetShowByIdAsync(id);

            if (show == null)
                return NotFound();

            return Ok(show);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostShow(ShowCreate showCreate)
        {
            Hall? hall = await _service.GetHallByIdAsync(showCreate.HallId);
            Movie? movie = await _service.GetMovieByIdAsync(showCreate.MovieId);

            if (hall == null)
                return BadRequest($"No hall with ID {showCreate.HallId} has been found.");
            if (movie == null)
                return BadRequest($"No movie with ID {showCreate.MovieId} has been found.");
        }
    }
}
