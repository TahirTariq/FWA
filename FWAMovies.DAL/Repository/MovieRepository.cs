using FWAMovies.DAL.Interface;
using FWAMovies.Model;
using System.Linq;

namespace FWAMovies.DAL.Repository
{
    public class MovieRepository : Repository<MovieContext, Movie>, IMovieRepository
    {
        public MovieRepository(MovieContext context) : base(context)
        {
        }

        public IQueryable<Movie> GetMovies()
        {
            return context.Movies;
        }
    }
}
