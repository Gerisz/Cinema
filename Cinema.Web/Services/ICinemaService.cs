using Cinema.Web.Models.Tables;

namespace Cinema.Web.Services
{
    public interface ICinemaService
    {
        public Task<Movie?> GetMovieByIdAsync(Int32 id);
        public IEnumerable<Movie> GetTodaysMovies();
        public Task<IEnumerable<Show>> GetTodaysShowsByMovieIdAsync(Int32 movieId);
        public Task<Show?> GetShowAsync(Int32 id);
        public Task ReserveSeatsAsync(IEnumerable<Int32> seatIds, String name, String phoneNumber);
    }
}
