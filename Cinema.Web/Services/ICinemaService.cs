using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables;

namespace Cinema.Web.Services
{
    public interface ICinemaService
    {
        public Task<List<ListShowDTO>> GetTodaysShowsAsync();
        public Task<ReserveSeatDTO?> GetShowAsync(Int32 id);
        public Task ReserveSeatAsync(Int32 showId,
            List<(Int32 row, Int32 column)> positions, String name, String phoneNumber);
    }
}
