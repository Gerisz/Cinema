using Cinema.Web.Models.DTOs;
using Cinema.Web.Models.Tables;

namespace Cinema.Web.Services
{
    public interface ICinemaService
    {
        public Task<List<ListShowDTO>> GetTodaysShowsAsync();
        public Task<ReserveSeatDTO?> GetShowAsync(Int32 id);
        public Task ReserveSeatsAsync(IEnumerable<Int32> seatIds, String name, String phoneNumber);
    }
}
