using FWAMovies.Model;
using System.Data.Entity;

namespace FWAMovies.DAL
{
    public class MovieContext : DbContext
    {
        public MovieContext() : base("FWAMoviesDb")
        {
            // Expecting changes during design phase.
            Database.SetInitializer(new DropCreateDatabaseAlways<MovieContext>());
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }       
        public DbSet<UserMovieReview> UserMovieReviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Properties()
                .Where(p => p.Name == "ID")
                .Configure(p => p.IsKey());
        }
    }
}
