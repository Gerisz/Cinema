using Cinema.Web.Models.Tables;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Cinema.Web.Models.DTOs
{
    public class SeatReserve
    {
        public Int32 ShowId { get; set; }
        public List<SeatIndex> Seats { get; set; } = [];
        public (Int32 RowCount, Int32 ColumnCount) HallSize { get; set; }

        [DisplayName("Név")]
        public String Name { get; set; } = null!;

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Telefonszám")]
        public String PhoneNumber { get; set; } = null!;

        public SeatReserve() { }

        public SeatReserve(Show show)
        {
            ShowId = show.Id;
            Seats = show.Seats
                    .AsQueryable()
                    .OrderBy(s => s.Row)
                    .ThenBy(s => s.Column)
                    .Select(SeatIndex.Projection)
                    .ToList();
            HallSize = new(show.Hall.RowCount, show.Hall.ColumnCount);
        }

        public static Expression<Func<Show, SeatReserve>> Projection { get; }
            = show => new SeatReserve()
            {

                ShowId = show.Id,
                Seats = show.Seats
                    .AsQueryable()
                    .OrderBy(s => s.Row)
                    .ThenBy(s => s.Column)
                    .Select(SeatIndex.Projection)
                    .ToList(),
                HallSize = new(show.Hall.RowCount, show.Hall.ColumnCount)
            };

        public static SeatReserve Create(Show show) => Projection.Compile().Invoke(show);
    }
}
