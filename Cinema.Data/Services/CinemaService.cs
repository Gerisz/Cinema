using Cinema.Data.Models;
using Cinema.Data.Models.DTOs;
using Cinema.Data.Models.Tables;
using Cinema.Data.Models.Tables.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Data.Services
{
    public class CinemaService : ICinemaService
    {
        private CinemaDbContext _context;

        public CinemaService(CinemaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Hall> GetHalls()
        {
            return _context.Halls.AsEnumerable();
        }

        public async Task<Hall?> GetHallByIdAsync(Int32 id)
        {
            return await _context.Halls.FindAsync(id);
        }

        public async Task<Movie?> GetMovieByIdAsync(Int32 id)
        {
            return await _context.Movies
                .Include(m => m.Shows)
                .ThenInclude(s => s.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public IEnumerable<Movie> GetTodaysMovies()
        {
            return _context.Movies
                .AsNoTracking()
                .Include(s => s.Shows)
                .Where(s => s.Shows
                    .Any(s => s.Start.Date == DateTime.Today))
                .OrderBy(s => s.Title)
                .AsSplitQuery();
        }

        public async Task<IEnumerable<Show>> GetTodaysShowsByMovieIdAsync(Int32 movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            return movie != null ? movie.Shows : [];
        }

        public async Task<Show?> GetShowByIdAsync(Int32 id)
        {
            return await _context.Shows
                .FindAsync(id);
        }

        public async Task<Movie?> CreateMovieAsync(Movie movie)
        {
            try
            {
                await _context.AddAsync(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return movie;
        }

        public async Task<Show?> CreateShowAsync(Show show)
        {
            try
            {
                await _context.AddAsync(show);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return show;
        }

        public async Task ReserveSeatsAsync
            (IEnumerable<Int32> seatIds, String name, String phoneNumber)
        {
            if (seatIds.Count() > 6)
                throw new ArgumentException("Can't reserve more than 6 seats");

            await _context.Seats.Where(s => seatIds.Contains(s.Id)).ForEachAsync(p =>
                (p.Status, p.ReservantName, p.ReservantPhoneNumber) =
                    (Status.Reserved, name, phoneNumber));

            await _context.SaveChangesAsync();
        }
    }
}
