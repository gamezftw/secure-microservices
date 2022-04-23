using System.Text.Json;
using Movies.Client.Model;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public MovieApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var response = await httpClient.PostAsJsonAsync<Movie>($"/api/movies/", movie);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync();
            var responseMovie = await JsonSerializer.DeserializeAsync<Movie>(content);

            return responseMovie;
        }

        public async Task DeleteMovie(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var response = await httpClient.GetFromJsonAsync<Movie>($"/api/movies/{id}");
        }

        public async Task<Movie> GetMovie(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var movie = await httpClient.GetFromJsonAsync<Movie>($"/api/movies/{id}");
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {

            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies/");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine(content);
            // var content = await response.Content.ReadAsStreamAsync();
            var movieList = JsonSerializer.Deserialize<List<Movie>>(content);
            return movieList;
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var response = await httpClient.PutAsJsonAsync<Movie>($"/api/movies/{movie.Id}", movie);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync();
            var responseMovie = await JsonSerializer.DeserializeAsync<Movie>(content);

            return responseMovie;
        }
    }
}
