using Cinema.Web.Models.Tables;
using Cinema.Web.Models.Tables.Enums;
using System.Linq.Expressions;

namespace Cinema.Web.Models.DTOs
{
    public class ListSeatDTO
    {
        public Int32 Id { get; set; }
        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
        public Boolean Reserved { get; set; }
        public Boolean Sold { get; set; }

        public static Expression<Func<Seat, ListSeatDTO>> Projection { get; }
            = seat => new ListSeatDTO()
            {
                Row = seat.Row,
                Column = seat.Column,
                Reserved = seat.Status == Status.Reserved || seat.Status == Status.Sold,
                Sold = seat.Status == Status.Sold
            };
    }
}
