using Cinema.Web.Models;
using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables;

namespace Cinema.Web.Services
{
    public class MovieService : IMovieService
    {
        private CinemaDbContext _context;

        public MovieService(CinemaDbContext context)
        {
            _context = context;
        }

        #region Read

        // TODO: implement this
        public List<NotImplementedException> GetTop5Posters()
        {
            throw new NotImplementedException();
        }

        public List<ListMovieDTO> GetTodaysMovies()
        {
            return _context.Movies
                .Where(m => m.Shows.Any(s => s.Start.Date == DateTime.Today))
                .Select(m => new ListMovieDTO
                {
                    Title = m.Title,
                    Start = m.Shows
                        .Where(s => s.Start.Date == DateTime.Today)
                        .Select(s => s.Start)
                        .ToList()
                })
                .ToList();
        }

        public Movie GetMovie(Int32 id)
        {
            return _context.Movies
                .Single(m => m.Id == id);
        }

        #endregion
    }
}
