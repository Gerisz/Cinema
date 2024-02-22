namespace Cinema.Web.Models.DTOs
{
    public class ListMovieDTO
    {
        public String Title { get; set; } = null!;
        public List<DateTime> Start { get; set; } = new();
    }
}
