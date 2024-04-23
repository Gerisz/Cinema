using Cinema.Data.Models.Tables;
using Cinema.Data.Models.Tables.Enums;

namespace Cinema.Data.Models.DTOs
{
    public class ShowIndex
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public int FreeSeatCount { get; set; }

        public static Func<Show, ShowIndex> Projection { get; }
            = show => new ShowIndex
            {
                Id = show.Id,
                Start = show.Start,
                FreeSeatCount = show.Seats.Count(s => s.Status == Status.Free)
            };
    }
}
