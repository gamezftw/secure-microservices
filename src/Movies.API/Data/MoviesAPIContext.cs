#nullable disable
using Microsoft.EntityFrameworkCore;

public class MoviesAPIContext : DbContext
    {
        public MoviesAPIContext (DbContextOptions<MoviesAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Movies.API.Model.Movie> Movie { get; set; }
    }
