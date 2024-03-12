using Cinema.Web.Models;
using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables;
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
                .Include(s => s.Hall)
                .Include(s => s.Movie)
                .Select(ReserveSeatDTO.Projection)
                .AsSplitQuery()
                .SingleOrDefaultAsync(s => s.ShowId == id);
        }

        public async Task ReserveSeatAsync(Int32 showId,
            List<(Int32 row, Int32 column)> positions, String name, String phoneNumber)
        {
            if (positions.Count > 6)
                throw new ArgumentException("Can't reserve more than 6 seats");

            var show = await _context.Shows.SingleAsync(s => s.Id == showId);

            positions.ForEach(p =>
            {
                var seat = show.Seats
                    .Single(s => (s.Row, s.Column) == p && s.ShowId == showId);

                (seat.Status, seat.ReservantName, seat.ReservantPhoneNumber) =
                    (Status.Reserved, name, phoneNumber);
            });

            await _context.SaveChangesAsync();
        }
    }
}
