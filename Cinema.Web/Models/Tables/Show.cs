namespace Cinema.Web.Models.Tables
{
    public class Show : Table
    {
        public String Title { get; set; } = null!;
        public DateTime Start { get; set; }
        
        public Int32 HallId { get; set; }
        public virtual Hall Hall { get; set; } = null!;

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
