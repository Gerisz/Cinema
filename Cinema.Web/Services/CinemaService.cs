using Cinema.Web.Models;
using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables;
using Cinema.Web.Models.Tables.Enums;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace Cinema.Web.Services
{
    public class CinemaService : ICinemaService
    {
        private CinemaDbContext _context;

        public CinemaService(CinemaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Show> GetTodaysShows()
        {
            return _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Where(s => s.Start.Date == DateTime.Today)
                .OrderBy(s => s.Movie.Title)
                .ThenBy(s => s.Start)
                .AsSplitQuery();
        }

        public async Task<Show?> GetShowAsync(Int32 id)
        {
            return await _context.Shows
                .FindAsync(id);
        }

        public async Task ReserveSeatsAsync(IEnumerable<Int32> seatIds, String name, String phoneNumber)
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
