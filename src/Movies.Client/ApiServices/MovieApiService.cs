using System.Diagnostics;
using System.Text.Json;
using IdentityModel.Client;
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

        public Task<Movie> CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovie(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {

            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies/");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movieList = JsonSerializer.Deserialize<IEnumerable<Movie>>(content);
            return movieList;

            // var apiCredentials = new ClientCredentialsTokenRequest
            // {
            //     Address = "http://localhost:5005/connect/token",
            //
            //     ClientId = "movieClient",
            //     ClientSecret = "secret",
            //
            //     Scope = "movieAPI"
            // };
            //
            // var client = new HttpClient();
            //
            // // just for test
            // var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5005");
            // if (disco.IsError)
            // {
            //     return null;
            // }
            //
            // var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiCredentials);
            // if (tokenResponse.IsError)
            // {
            //     return null;
            // }
            //
            //
            // var apiClient = new HttpClient();
            //
            // apiClient.SetBearerToken(tokenResponse.AccessToken);
            //
            // // var response = await apiClient.GetAsync("http://localhost:5001/api/movies");
            // // var content = await response.Content.ReadAsStringAsync();
            // // response.EnsureSuccessStatusCode();
            //
            // var movieList = await apiClient.GetFromJsonAsync<IEnumerable<Movie>>("http://localhost:5001/api/movies");
            //
            // if (movieList != null)
            //     return movieList;
            // else
            //     return null;
            //
            // var movieList = new List<Movie>();
            // movieList.Add(
            //     new Movie
            //     {
            //         Id = 1,
            //         Genre = "Comics",
            //         Title = "asd",
            //         Rating = "9.2",
            //         ImageUrl = "images/src",
            //         ReleaseDate = DateTime.Now,
            //         Owner = "swn",
            //     }
            // );
            // return await Task.FromResult(movieList);
        }

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
