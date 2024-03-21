using Cinema.Data.Models.Tables;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cinema.Web.Models
{
    public class MovieDetails
    {
        public int Id { get; set; }
        [DisplayName("Cím")]
        public string Title { get; set; } = null!;
        [DisplayName("Rendező")]
        public string Director { get; set; } = null!;
        [DisplayName("Szinopszis")]
        public string Synopsis { get; set; } = null!;
        [DisplayName("Hossz (perc)")]
        public int Length { get; set; }
        [DisplayName("Mai kezdés időpontjai")]
        public IEnumerable<ShowIndex> Starts { get; set; } = [];
        public byte[]? Image { get; set; }

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
                    .OrderBy(s => s.Start),
                Image = movie.Image
            };

        public static MovieDetails Create(Movie movie) => Projection.Compile().Invoke(movie);
    }
}
