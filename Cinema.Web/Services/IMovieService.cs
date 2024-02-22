using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables;

namespace Cinema.Web.Services
{
    public interface IMovieService
    {
        public Task<List<NotImplementedException>> GetTop5PostersAsync();
        public Task<List<ListMovieDTO>> GetTodaysMoviesAsync();
        public Task<Movie> GetMovieAsync(Int32 id);
    }
}
