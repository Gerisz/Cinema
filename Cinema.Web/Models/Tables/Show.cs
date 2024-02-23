namespace Cinema.Web.Models.Tables
{
    public class Show : Table
    {
        public DateTime Start { get; set; }

        public Int32 MovieId { get; set; }
        public virtual Movie Movie { get; set; } = null!;
        
        public Int32 HallId { get; set; }
        public virtual Hall Hall { get; set; } = null!;

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
