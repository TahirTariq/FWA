using FWAMovies.DAL.Interface;
using FWAMovies.Model;
using System.Data.Entity;

namespace FWAMovies.DAL.Context
{
    public class MovieContext : DbContext, IMovieContext
    {
        public MovieContext(string connnectionString) : base(connnectionString)
        {
            Database.SetInitializer(new FWAMoviesDBInitializer<MovieContext>());
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
