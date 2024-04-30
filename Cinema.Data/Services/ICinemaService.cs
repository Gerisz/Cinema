using Cinema.Data.Models.Tables;

namespace Cinema.Data.Services
{
    public interface ICinemaService
    {
        public Task<Movie?> GetMovieByIdAsync(Int32 id);
        public IEnumerable<Movie> GetTodaysMovies();
        public Task<IEnumerable<Show>> GetTodaysShowsByMovieIdAsync(Int32 movieId);
        public Task<Show?> GetShowByIdAsync(Int32 id);
        public Task ReserveSeatsAsync(IEnumerable<Int32> seatIds, String name, String phoneNumber);
        public Task<Movie?> CreateMovieAsync(Movie movie);
        public Task<Show?> CreateShowAsync(Show show);
        Task<Hall?> GetHallByIdAsync(Int32 id);
        IEnumerable<Hall> GetHalls();
    }
}
