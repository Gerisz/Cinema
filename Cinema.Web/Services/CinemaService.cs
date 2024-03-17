using Cinema.Web.Models;
using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Web.Services
{
    public class CinemaService : ICinemaService
    {
        private CinemaDbContext _context;

        public CinemaService(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListShowDTO>> GetTodaysShowsAsync()
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Where(s => s.Start.Date == DateTime.Today)
                .OrderBy(s => s.Movie.Title)
                .ThenBy(s => s.Start)
                .Select(ListShowDTO.Projection)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<ReserveSeatDTO?> GetShowAsync(Int32 id)
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Seats)
                .Include(s => s.Hall)
                .Include(s => s.Movie)
                .Select(ReserveSeatDTO.Projection)
                .AsSplitQuery()
                .SingleOrDefaultAsync(s => s.ShowId == id);
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
