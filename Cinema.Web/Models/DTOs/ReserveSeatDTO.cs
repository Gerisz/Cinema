using Cinema.Web.Models.Tables;
using System.Linq.Expressions;

namespace Cinema.Web.Models.DTOs
{
    public class ReserveSeatDTO
    {
        public Int32 ShowId { get; set; }
        public String Title { get; set; } = null!;
        public DateTime Start { get; set; }
        public List<ListSeatDTO> Seats { get; set; } = [];
        public String Name { get; set; } = null!;
        public String PhoneNumber { get; set; } = null!;


        public static Expression<Func<Show, ReserveSeatDTO>> Projection { get; }
            = show => new ReserveSeatDTO()
            {

                ShowId = show.Id,
                Title = show.Movie.Title,
                Start = show.Start,
                Seats = show.Seats
                    .AsQueryable()
                    .Select(ListSeatDTO.Projection)
                    .OrderBy(s => s.Row)
                    .ThenBy(s => s.Column)
                    .ToList()
            };

        public static ReserveSeatDTO Create(Show show) => Projection.Compile().Invoke(show);
    }
}
