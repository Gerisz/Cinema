using Cinema.Data.Models.Tables.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Data.Models.Tables
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

        public static ICollection<Seat> GenerateSeats(Show show)
        {
            Hall hall = show.Hall;
            ICollection<Seat> seats = new List<Seat>(hall.RowCount * hall.ColumnCount);

            for (Int32 i = 0; i < show.Hall.RowCount; i++)
                for (Int32 j = 0; j < show.Hall.ColumnCount; j++)
                    seats.Add(new Seat
                    {
                        Show = show,
                        Hall = show.Hall,
                        Row = i + 1,
                        Column = j + 1,
                        Status = Status.Free
                    });

            return seats;
        }
    }
}
