namespace Cinema.Web.Models.DTOs
{
    public class ListSeatDTO
    {
        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
        public String Status { get; set; } = null!;
    }
}
