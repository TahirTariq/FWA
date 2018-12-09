using FWAMovies.DAL.Interface;
using FWAMovies.Model;
using System.Collections.Generic;
using System.Linq;

namespace FWAMovies.DAL.Repository
{
    public class MovieRepository : Repository<IMovieContext, Movie>, IMovieRepository
    {
        public MovieRepository(IMovieContext context) : base(context)
        {
        }

        /// <summary>
        /// Just a simple implementation to show class specific method.
        /// </summary>
        /// <returns><see cref="IQueryable<Movie>"/></returns>
        public IQueryable<Movie> GetMovies()
        {
            return context.Movies;
        }

        public Movie GetById(int movieId)
        {
            return context.Movies.FirstOrDefault(m => m.ID == movieId);
        }
        public IEnumerable<Movie> GetTopMoviesByUserScore(int userId)
        {
            IQueryable<Movie> result = from m in context.Movies
                                       from r in m.Reviews
                                       where r.UserID == userId
                                       orderby r.Rating descending
                                       orderby m.Title ascending
                                       select r.ReviewMovie;

            return result;
        }
    }
}
