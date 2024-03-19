using Cinema.Web.Models.Tables;
using Cinema.Web.Models.Tables.Enums;
using System.Linq.Expressions;

namespace Cinema.Web.Models.DTOs
{
    public class ListSeatDTO
    {
        public Int32 Id { get; set; }
        public Boolean Reserved { get; set; }
        public Boolean ToReserve { get; set; } = false;

        public static Expression<Func<Seat, ListSeatDTO>> Projection { get; }
            = seat => new ListSeatDTO()
            {
                Id = seat.Id,
                Reserved = seat.Status == Status.Reserved || seat.Status == Status.Sold
            };
    }
}
