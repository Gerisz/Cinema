using Cinema.Web.Models.Tables;
using Cinema.Web.Models.Tables.Enums;
using System.Linq.Expressions;

namespace Cinema.Web.Models.DTOs
{
    public class SeatIndex
    {
        public Int32 Id { get; set; }
        public Boolean Reserved { get; set; }
        public Boolean ToReserve { get; set; } = false;

        public static Expression<Func<Seat, SeatIndex>> Projection { get; }
            = seat => new SeatIndex()
            {
                Id = seat.Id,
                Reserved = seat.Status == Status.Reserved || seat.Status == Status.Sold
            };
    }
}
