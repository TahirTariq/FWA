using FWAMovies.Model;
using System.Linq;

namespace FWAMovies.DAL.Interface
{
    public interface IMovieRepository : IRepository<Movie>
    {
        // Just for testing
        IQueryable<Movie> GetMovies();
    }
}
