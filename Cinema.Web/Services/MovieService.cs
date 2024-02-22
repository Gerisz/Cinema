using Cinema.Web.Models;
using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<NotImplementedException>> GetTop5PostersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ListMovieDTO>> GetTodaysMoviesAsync()
        {
            return await _context.Movies
                .Where(m => m.Shows.Any(s => s.Start.Date == DateTime.Today))
                .OrderBy(m => m.Title)
                .Select(m => new ListMovieDTO
                {
                    Title = m.Title,
                    Start = m.Shows
                        .Where(s => s.Start.Date == DateTime.Today)
                        .Select(s => s.Start)
                        .ToList()
                })
                .ToListAsync();
        }

        public async Task<Movie> GetMovieAsync(Int32 id)
        {
            return await _context.Movies
                .SingleAsync(m => m.Id == id);
        }

        #endregion
    }
}
