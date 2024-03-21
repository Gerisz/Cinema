using Cinema.Data.Models.Tables;
using Cinema.Data.Models.Tables.Enums;
using System.Linq.Expressions;

namespace Cinema.Web.Models
{
    public class SeatIndex
    {
        public int Id { get; set; }
        public bool Reserved { get; set; }
        public bool ToReserve { get; set; } = false;

        public static Expression<Func<Seat, SeatIndex>> Projection { get; }
            = seat => new SeatIndex()
            {
                Id = seat.Id,
                Reserved = seat.Status == Status.Reserved || seat.Status == Status.Sold
            };
    }
}
