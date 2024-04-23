using Cinema.Data.Models.Tables;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Cinema.Data.Models.DTOs
{
    public class MovieIndex
    {
        public int Id { get; set; }
        [DisplayName("Cím")]
        public string Title { get; set; } = null!;
        [DisplayName("Kezdés időpontjai")]
        public IEnumerable<DateTime> Starts { get; set; } = [];
        public DateTime Entry { get; set; }
        public byte[]? Image { get; set; }

        public static Expression<Func<Movie, MovieIndex>> Projection { get; }
            = movie => new MovieIndex()
            {
                Id = movie.Id,
                Title = movie.Title,
                Starts = movie.Shows
                    .Select(s => s.Start)
                    .OrderBy(t => t)
                    .AsEnumerable(),
                Entry = movie.Entry,
                Image = movie.Image
            };
    }
}
