using Microsoft.IdentityModel.Tokens;

namespace Cinema.Data.Models.Tables
{
    public class Show : Table
    {
        public DateTime Start { get; set; }

        public Int32 MovieId { get; set; }
        public virtual Movie Movie { get; set; } = null!;

        public Int32 HallId { get; set; }
        public virtual Hall Hall { get; set; } = null!;

        public virtual ICollection<Seat> Seats { get; set; } = [];

        public Show() { }

        public Show(DateTime start, Movie movie, Hall hall, ICollection<Seat>? seats = null)
        {
            Start = start;
            Movie = movie;
            Hall = hall;
            Seats = seats.IsNullOrEmpty()
                ? Seat.GenerateSeats(this)
                : seats!;
        }

        public Show Update(Show show)
        {
            Start = show.Start;
            Movie = show.Movie;
            Hall = show.Hall;
            Seats = show.Seats;

            return this;
        }
    }
}
