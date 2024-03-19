using Cinema.Web.Models.Tables;
using Cinema.Web.Models.Tables.Enums;

namespace Cinema.Web.Models.DTOs
{
    public class ShowIndex
    {
        public Int32 Id { get; set; }
        public DateTime Start { get; set; }
        public Int32 FreeSeatCount { get; set; }

        public static Func<Show, ShowIndex> Projection { get; }
            = show => new ShowIndex
            {
                Id = show.Id,
                Start = show.Start,
                FreeSeatCount = show.Seats.Count(s => s.Status == Status.Free)
            };
    }
}
