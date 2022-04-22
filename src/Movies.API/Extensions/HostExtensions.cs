using Movies.API.Data;

namespace Movies.API.Extensions
{
    public static class HostExtensions
    {
        public static void SeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var moviesAPIContext = services.GetRequiredService<MoviesAPIContext>();
                MoviesContextSeed.SeedAsync(moviesAPIContext);
            }
        }
    }
}
