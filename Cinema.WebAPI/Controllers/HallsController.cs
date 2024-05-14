using Cinema.Data.Models;
using Cinema.Data.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallsController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public HallsController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HallDTO>>> GetHallsAsync()
        {
            var halls = _context.Halls
                .AsNoTracking()
                .Select(HallDTO.Projection)
                .ToListAsync();

            return Ok(await halls);
        }
    }
}
