using Cinema.Data.Models.Tables;
using System.Linq.Expressions;

namespace Cinema.WebAPI.Controllers
{
    public class ShowDetails
    {
        public DateTime Start { get; set; }
        public Int32 MovieId { get; set; }
        public Int32 HallId { get; set; }
        public IEnumerable<Int32> SeatIds { get; set; } = [];

        public static Expression<Func<Show, ShowDetails>> Projection { get; }
            = show => new ShowDetails()
            {
                Start = show.Start,
                MovieId = show.MovieId,
                HallId = show.HallId,
                SeatIds = show.Seats.Select(s => s.Id)
            };

        public static ShowDetails Create(Show show) => Projection.Compile().Invoke(show);
    }
}