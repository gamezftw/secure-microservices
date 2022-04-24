using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Model;
using System.Text.Json;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var response = await httpClient.PostAsJsonAsync<Movie>($"/api/movies/", movie);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStreamAsync();
            var responseMovie = await System.Text.Json.JsonSerializer.DeserializeAsync<Movie>(content);

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

            var movieList = await httpClient.GetFromJsonAsync<IEnumerable<Movie>>("/api/movies/");

            return movieList;

            // var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies/");
            //
            // var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            //
            // response.EnsureSuccessStatusCode();
            //
            // var content = await response.Content.ReadAsStreamAsync();
            // var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; // doesn't work without this...
            //
            // var movieList = JsonSerializer.Deserialize<IEnumerable<Movie>>(content, options)!;
            // 
            // return movieList;
        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var idpClient = _httpClientFactory.CreateClient("IDPClient");

            var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();

            if (metaDataResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the token");
            }

            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfoResponse = await idpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = metaDataResponse.UserInfoEndpoint,
                Token = accessToken
            });

            if (userInfoResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while geting user info");
            }

            var userInfoDictionary = new Dictionary<string, string>();

            foreach (var claim in userInfoResponse.Claims)
            {
                userInfoDictionary.Add(claim.Type, claim.Value);
            }

            return new UserInfoViewModel(userInfoDictionary);
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var response = await httpClient.PutAsJsonAsync<Movie>($"/api/movies/{movie.Id}", movie);
            response.EnsureSuccessStatusCode();

            return movie;
        }
    }
}
