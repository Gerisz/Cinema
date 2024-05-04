using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Data.Services;
using Cinema.Data.Models.DTOs;

namespace Cinema.Web.Controllers
{
    public class ShowsController : Controller
    {
        private readonly CinemaDbContext _context;

        public ShowsController(CinemaDbContext service)
        {
            _context = service;
        }
        /*
        // GET: Shows
        public async Task<IActionResult> Index()
        {
            return View(_context.GetTodaysShows()
                .AsQueryable()
                .Select(ListShowDTO.Projection));
        }

        // GET: Shows/Details/5
        public async Task<IActionResult> Details(Int32 id)
        {
            var show = await _context.GetShowAsync(id);
            if (show == null)
                return NotFound();
            return View(show);
        }
        
        // GET: Shows/Create
        public IActionResult Create()
        {
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id");
            return View();
        }

        // POST: Shows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Start,MovieId,HallId,Id")] Show show)
        {
            if (ModelState.IsValid)
            {
                _context.Add(show);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", show.HallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", show.MovieId);
            return View(show);
        }*/

        // GET: Shows/Edit/5
        public async Task<IActionResult> Edit(Int32 id)
        {
            var show = await _context.GetShowByIdAsync(id);
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

            if (ModelState.IsValid)
            {
                try
                {
                    var toReserve = dto.Seats
                        .Where(s => s.ToReserve && !s.Reserved)
                        .Select(s => s.Id);
                    await _context.ReserveSeatsAsync(toReserve, dto.Name, dto.PhoneNumber);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowExists(dto.ShowId))
                        return NotFound();
                    else
                        throw;
                }
                catch (ArgumentException)
                {
                    return BadRequest("Can't reserve more than 6 seats");
                }
                return RedirectToAction("Index", "Movies");
            }
            return await Edit(id);
        }
        /*
        // GET: Shows/Delete/5
        public async Task<IActionResult> Delete(Int32 id)
        {
            var show = await _context.GetShowAsync(id);
            if (show == null)
                return NotFound();

            return View(show);
        }

        // POST: Shows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show != null)
            {
                _context.Shows.Remove(show);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/

        private bool ShowExists(int id)
        {
            return _context.GetShowByIdAsync(id) != null;
        }
    }
}
