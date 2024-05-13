using Cinema.Data.Models.DTOs;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Cinema.Admin.Model
{
    public class CinemaAPIService
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

        #region Movies

        public async Task<IEnumerable<MovieDTO>> LoadMoviesAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Movies/");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<IEnumerable<MovieDTO>>();

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }

        public async Task CreateMovieAsync(MovieDTO movie)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Movies/", movie);
            movie.Id = (await response.Content.ReadAsAsync<MovieDTO>()).Id;

            if (!response.IsSuccessStatusCode)
                throw new NetworkException("Service returned response: " + response);
        }

        public async Task UpdateMovieAsync(MovieDTO movie)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"api/Movies/{movie.Id}", movie);
            movie.Id = (await response.Content.ReadAsAsync<MovieDTO>()).Id;

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
    }
}
