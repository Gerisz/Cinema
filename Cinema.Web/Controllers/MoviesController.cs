using Cinema.Data.Models.DTOs;
using Cinema.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Web.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ICinemaService _service;

        public MoviesController(ICinemaService service)
        {
            _service = service;
        }

        public async Task<IActionResult?> DisplayImage(Int32 id)
        {
            var movie = await _service.GetMovieByIdAsync(id);
            if (movie != null && movie.Image != null)
                return File(movie.Image, "image/png");
            return null;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var movies = await _service.GetTodaysMovies()
                .AsQueryable()
                .Select(MovieIndex.Projection)
                .ToListAsync();
            movies
                .ForEach(m => m.Starts = m.Starts
                    .Where(s => s.Date == DateTime.Today));
            return View(movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(Int32 id)
        {
            var movie = await _service.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.Shows = movie.Shows.Where(s => s.Start.Date == DateTime.Today).ToList();

            return View(MovieDetails.Create(movie));
        }
        /*
        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Director,Synopsis,Length,Entry,Id")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Director,Synopsis,Length,Entry,Id")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }*/
    }
}
