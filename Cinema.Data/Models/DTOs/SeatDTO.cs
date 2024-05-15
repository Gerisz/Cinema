using Cinema.Data.Models.Tables;
using Cinema.Data.Models.Tables.Enums;
using System.Linq.Expressions;

namespace Cinema.Data.Models.DTOs
{
    public class SeatDTO
    {
        public Int32 Id { get; set; }
        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
        public Status Status { get; set; }
        public String? ReservantName { get; set; }
        public String? ReservantPhoneNumber { get; set; }

        public static Expression<Func<Seat, SeatDTO>> Projection { get; }
            = seat => new SeatDTO()
            {
                Id = seat.Id,
                Row = seat.Row,
                Column = seat.Column,
                Status = seat.Status,
                ReservantName = seat.ReservantName,
                ReservantPhoneNumber = seat.ReservantPhoneNumber
            };
    }
}
