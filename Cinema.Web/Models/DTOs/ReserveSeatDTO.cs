using Cinema.Web.Models.Tables;
using System.Linq.Expressions;

namespace Cinema.Web.Models.DTOs
{
    public class ReserveSeatDTO
    {
        public Int32 ShowId { get; set; }
        public List<ListSeatDTO> Seats { get; set; } = [];
        public (Int32 RowCount, Int32 ColumnCount) HallSize { get; set; }
        public String Name { get; set; } = null!;
        public String PhoneNumber { get; set; } = null!;

        public ReserveSeatDTO() { }

        public ReserveSeatDTO(Show show)
        {
            ShowId = show.Id;
            Seats = show.Seats
                    .AsQueryable()
                    .OrderBy(s => s.Row)
                    .ThenBy(s => s.Column)
                    .Select(ListSeatDTO.Projection)
                    .ToList();
            HallSize = new(show.Hall.RowCount, show.Hall.ColumnCount);
        }

        public static Expression<Func<Show, ReserveSeatDTO>> Projection { get; }
            = show => new ReserveSeatDTO()
            {

                ShowId = show.Id,
                Seats = show.Seats
                    .AsQueryable()
                    .OrderBy(s => s.Row)
                    .ThenBy(s => s.Column)
                    .Select(ListSeatDTO.Projection)
                    .ToList(),
                HallSize = new(show.Hall.RowCount, show.Hall.ColumnCount)
            };

        public static ReserveSeatDTO Create(Show show) => Projection.Compile().Invoke(show);
    }
}
