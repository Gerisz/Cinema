using Cinema.Data.Models;
using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly CinemaDbContext _context;

        public SeatsController(CinemaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatDTO>>> GetSeatsAsync()
        {
            var seats = _context.Seats
                .AsNoTracking()
                .Select(SeatDTO.Projection)
                .ToListAsync();

            return Ok(await seats);
        }

        [Authorize]
        [HttpPut("{seatId}")]
        public async Task<IActionResult> SellSeatAsync(Int32 seatId)
        {
            var seat = await _context.Seats.FindAsync(seatId);

            if (seat == null)
                return NotFound();
            if (seat.Status == Status.Sold)
                return BadRequest("Can't sell an already sold seat!");

            seat.Status = Status.Sold;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
