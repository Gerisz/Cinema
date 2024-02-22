using Cinema.Web.Models.DTOs;

namespace Cinema.Web.Services
{
    public interface IShowService
    {
        public Task<List<ListSeatDTO>> GetSeatsByShowIdAsync(Int32 showId);
        public Task ReserveSeatAsync(Int32 showId,
           List<(Int32 row, Int32 column)> positions, String name, String phoneNumber);
    }
}
