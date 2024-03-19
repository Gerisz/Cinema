using Cinema.Web.Models.Tables;
using Cinema.Web.Models.Tables.Enums;
using System.Linq.Expressions;

namespace Cinema.Web.Models.DTOs
{
    public class ListShowDTO
    {
        public Int32 Id { get; set; }
        public String Title { get; set; } = null!;
        public DateTime Start { get; set; }
        public Int32 FreeSeatCount { get; set; }

        public static Expression<Func<Show, ListShowDTO>> Projection { get; }
            = show => new ListShowDTO()
            {
                Id = show.Id,
                Title = show.Movie.Title,
                Start = show.Start,
                FreeSeatCount = show.Seats.Count(s => s.Status == Status.Free)
            };

        public static ListShowDTO Create(Show show) => Projection.Compile().Invoke(show);
    }
}
