using Cinema.Web.Models.Tables.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Web.Models.Tables
{
    public class Seat : Table
    {
        public Int32? ShowId { get; set; }
        public virtual Show? Show { get; set; } = null!;

        public Int32? HallId { get; set; }
        public virtual Hall? Hall { get; set; } = null!;

        public Int32 Row { get; set; }
        public Int32 Column { get; set; }

        public Status Status { get; set; } = Status.Free;

        public String? ReservantName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public String? ReservantPhoneNumber { get; set; }
    }
}
