using Cinema.Data.Models.Tables;
using System.Linq.Expressions;

namespace Cinema.Data.Models.DTOs
{
    public class ShowDTO
    {
        public Int32 Id { get; set; }
        public DateTime Start { get; set; }
        public MovieDTO Movie { get; set; } = null!;
        public HallDTO Hall { get; set; } = null!;
        public IEnumerable<Int32> SeatIds { get; set; } = [];

        public static Expression<Func<Show, ShowDTO>> Projection { get; }
            = show => new ShowDTO
            {
                Id = show.Id,
                Start = show.Start,
                Movie = MovieDTO.Create(show.Movie),
                Hall = HallDTO.Create(show.Hall),
                SeatIds = show.Seats.Select(s => s.Id)
            };

        public static ShowDTO Create(Show show) => Projection.Compile().Invoke(show);
    }
}
