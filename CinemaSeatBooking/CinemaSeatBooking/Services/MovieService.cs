using CinemaSeatBooking.Models;
using System.Net.Http.Json;

namespace CinemaSeatBooking.Services
{
    public class MovieService
    {
        private readonly HttpClient _http;

        public MovieService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await _http.GetFromJsonAsync<List<Movie>>("data/movies.json") ?? new();
        }

        public async Task<Movie?> GetMovieByIdAsync(string id)
        {
            var movies = await GetMoviesAsync();
            return movies.FirstOrDefault(m => m.Id == id);
        }
    }
}
