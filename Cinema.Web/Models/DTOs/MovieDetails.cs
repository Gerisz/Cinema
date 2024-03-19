using Cinema.Web.Models.Tables;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cinema.Web.Models.DTOs
{
    public class MovieDetails
    {
        public Int32 Id { get; set; }
        [DisplayName("Cím")]
        public String Title { get; set; } = null!;
        [DisplayName("Rendező")]
        public String Director { get; set; } = null!;
        [DisplayName("Szinopszis")]
        public String Synopsis { get; set; } = null!;
        [DisplayName("Hossz (perc)")]
        public Int32 Length { get; set; }
        [DisplayName("Mai kezdés időpontjai")]
        public IEnumerable<ShowIndex> Starts { get; set; } = [];

        public static Expression<Func<Movie, MovieDetails>> Projection { get; }
            = movie => new MovieDetails()
            {
                Id = movie.Id,
                Title = movie.Title,
                Director = movie.Director,
                Synopsis = movie.Synopsis,
                Length = movie.Length,
                Starts = movie.Shows
                    .Select(ShowIndex.Projection)
                    .OrderBy(s => s.Start)
            };

        public static MovieDetails Create(Movie movie) => Projection.Compile().Invoke(movie);
    }
}
