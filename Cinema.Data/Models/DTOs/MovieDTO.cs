using Cinema.Data.Models.Tables;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Cinema.Data.Models.DTOs
{
    public class MovieDTO
    {
        public Int32 Id { get; set; }
        public String Title { get; set; } = null!;
        public String Director { get; set; } = null!;

        [DataType(DataType.MultilineText)]
        public String Synopsis { get; set; } = null!;

        public Int32 Length { get; set; }

        public Byte[]? Image { get; set; }

        public DateTime Entry { get; set; }

        public static Expression<Func<Movie, MovieDTO>> Projection { get; }
            = movie => new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Synopsis = movie.Synopsis,
                Director = movie.Director,
                Length = movie.Length,
                Image = movie.Image,
                Entry = movie.Entry
            };

        public static MovieDTO Create(Movie movie) => Projection.Compile().Invoke(movie);
    }
}
