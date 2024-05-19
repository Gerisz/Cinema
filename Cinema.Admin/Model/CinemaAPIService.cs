using Azure;
using Cinema.Data.Models.DTOs;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Cinema.Admin.Model
{
    public class CinemaAPIService : IDisposable
    {
        private readonly HttpClient _client;

        public CinemaAPIService(Uri baseAddress)
        {
            _client = new HttpClient
            {
                BaseAddress = baseAddress
            };
        }

        #region Authentication

        public async Task<Boolean> LoginAsync(String userName, String password)
        {
            UserLogin user = new UserLogin
            {
                UserName = userName,
                Password = password
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Employee/Login", user);

            if (response.IsSuccessStatusCode)
                return true;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return false;

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task LogoutAsync()
        {
            HttpResponseMessage response = await _client.PostAsync("api/Employee/Logout", null);

            if (response.IsSuccessStatusCode)
                return;

            throw new NetworkException("Service returned response: " + response);
        }

        #endregion

        #region Halls

        public async Task<IEnumerable<HallDTO>> LoadHallsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Halls/");

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response.StatusCode);

            return await response.Content.ReadAsAsync<IEnumerable<HallDTO>>();
        }

        #endregion

        #region Movies

        public async Task<IEnumerable<MovieDTO>> LoadMoviesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Movies/");

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response.StatusCode);

            return await response.Content.ReadAsAsync<IEnumerable<MovieDTO>>();
        }

        public async Task CreateMovieAsync(MovieDTO movie)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Movies/", movie);

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response);

            movie.Id = (await response.Content.ReadAsAsync<MovieDTO>()).Id;
        }

        public async Task UpdateMovieAsync(MovieDTO movie)
        {
            HttpResponseMessage response =
                await _client.PutAsJsonAsync($"api/Movies/{movie.Id}", movie);

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response);
        }

        public async Task DeleteMovieAsync(Int32 id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/Movies/{id}");

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response);
        }

        #endregion

        #region Seats

        public async Task<IEnumerable<SeatDTO>> LoadSeatsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Seats");

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response.StatusCode);

            return await response.Content.ReadAsAsync<IEnumerable<SeatDTO>>();
        }

        public async Task SellSeatAsync(SeatDTO seat)
        {
            HttpResponseMessage response =
                await _client.PutAsJsonAsync($"api/Seats/{seat.Id}", "");

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response);
        }

        #endregion

        #region Shows

        public async Task<IEnumerable<ShowDTO>> LoadShowsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Shows/");

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response.StatusCode);

            return await response.Content.ReadAsAsync<IEnumerable<ShowDTO>>();
        }

        public async Task CreateShowAsync(ShowDTO show)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Shows/", show);

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response);

            show.Id = (await response.Content.ReadAsAsync<ShowDTO>()).Id;
        }

        public async Task UpdateShowAsync(ShowDTO show)
        {
            HttpResponseMessage response =
                await _client.PutAsJsonAsync($"api/Shows/{show.Id}", show);

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response);
        }

        public async Task DeleteShowAsync(Int32 id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/Shows/{id}");

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
