using FWAMovies.Model;
using System.Data.Entity;

namespace FWAMovies.DAL.Interface
{
    public interface IMovieContext : IDbContext
    {
        DbSet<Movie> Movies { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserMovieReview> UserMovieReviews { get; set; }
    }
}
