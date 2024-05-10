using Cinema.Data.Models;
using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Web.Controllers
{
    public class ShowsController : Controller
    {
        private readonly CinemaDbContext _context;

        public ShowsController(CinemaDbContext service)
        {
            _context = service;
        }

        // GET: Shows/Edit/5
        public async Task<IActionResult> Edit(Int32 id)
        {
            var show = await _context.Shows.FindAsync(id);

            if (show == null)
                return NotFound();

            var reserveSeatDTO = SeatReserve.Create(show);

            ViewData["Title"] = show.Movie.Title.ToString();
            ViewData["Start"] = show.Start.ToString();

            return View(reserveSeatDTO);
        }

        // POST: Shows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32 id, SeatReserve dto)
        {
            if (id != dto.ShowId)
                return NotFound();

            if (dto.Seats.Where(s => s.ToReserve).Count() > 6)
                return await Edit(id);

            if (ModelState.IsValid)
            {
                try
                {
                    var seatIds = dto.Seats
                        .Where(s => s.ToReserve && !s.Reserved)
                        .Select(s => s.Id);

                    var toReserve = _context.Seats.Where(s => seatIds.Contains(s.Id));

                    if (toReserve.Any(s => s.Status != Status.Free))
                        return await Edit(id);

                    await _context.Seats.Where(s => seatIds.Contains(s.Id)).ForEachAsync(p =>
                        (p.Status, p.ReservantName, p.ReservantPhoneNumber) =
                            (Status.Reserved, dto.Name, dto.PhoneNumber));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ShowExistsAsync(dto.ShowId)))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index", "Movies");
            }
            return await Edit(id);
        }

        private async Task<Boolean> ShowExistsAsync(Int32 id)
        {
            return (await _context.Shows.FindAsync(id)) != null;
        }
    }
}
