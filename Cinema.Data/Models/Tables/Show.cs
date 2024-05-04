namespace Cinema.Data.Models.Tables
{
    public class Show : Table
    {
        public DateTime Start { get; set; }

        public Int32 MovieId { get; set; }
        public virtual Movie Movie { get; set; } = null!;
        
        public Int32 HallId { get; set; }
        public virtual Hall Hall { get; set; } = null!;

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public Show() { }

        public Show(DateTime start, Movie movie, Hall hall)
        {
            Start = start;
            Movie = movie;
            Hall = hall;
            Seats = Seat.GenerateSeats(this);
        }
    }
}
