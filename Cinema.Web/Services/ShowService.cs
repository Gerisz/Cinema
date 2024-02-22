using Cinema.Web.Models;
using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables.EnumTables;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Web.Services
{
    public class ShowService : IShowService
    {
        private CinemaDbContext _context;

        public ShowService(CinemaDbContext context)
        {
            _context = context;
        }

        #region Read

        public async Task<List<ListSeatDTO>> GetSeatsByShowIdAsync(Int32 showId)
        {
            return await _context.Seats
                .Where(s => s.ShowId == showId)
                .Select(s => new ListSeatDTO()
                {
                    Row = s.Row,
                    Column = s.Column,
                    Status = s.Status.Value
                }).ToListAsync();
        }

        #endregion

        #region Update

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

                (seat.StatusId, seat.ReservantName, seat.ReservantPhoneNumber) =
                    ((Int32)StatusEnum.Reserved, name, phoneNumber);
            });

        }

        #endregion
    }
}
