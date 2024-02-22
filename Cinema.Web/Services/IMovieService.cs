using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables;

namespace Cinema.Web.Services
{
    public interface IMovieService
    {
        public List<NotImplementedException> GetTop5Posters();
        public List<ListMovieDTO> GetTodaysMovies();
        public Movie GetMovie(Int32 id);
    }
}
