namespace Cinema.Data.Models.DTOs
{
    public class ShowCreate
    {
        public Int32 HallId { get; set; }
        public Int32 MovieId { get; set; }
        public DateTime Start { get; set; }
    }
}
