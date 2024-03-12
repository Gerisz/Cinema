namespace Cinema.Web.Models.DTOs
{
    public class ReserveSeatDTO
    {
        public Int32 ShowId { get; set; }
        public List<(Int32 Row, Int32 Column)> Positions { get; set; } = [];
        public String Name { get; set; } = null!;
        public String PhoneNumber { get; set; } = null!;
    }
}
