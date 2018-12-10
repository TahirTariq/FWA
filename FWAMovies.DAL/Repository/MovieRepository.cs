using FWAMovies.DAL.Interface;
using FWAMovies.Model;
using FWAMovies.Model.Dto;
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
                                       orderby r.Rating descending,
                                       m.Title ascending
                                       select r.ReviewMovie;

            return result;
        }

        public IEnumerable<Movie> GetMoviesBy(MovieFilter filter)
        {
            var predicate = PredicateBuilder.True<Movie>();

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                predicate = predicate.Or(m => m.Title.Contains(filter.Title));
            }

            if (filter.YearOfRelease != null)
            {
                predicate = predicate.Or(m => m.YearOfRelease == filter.YearOfRelease);
            }

            if (filter.Genre != null && filter.Genre.Length > 0)
            {
                foreach (string genre in filter.Genre)
                {
                    predicate = predicate.Or(m => m.Genres.Contains(genre));
                }
            }

            return context.Movies.Where(predicate)
                .OrderByDescending(m => m.AverageRating)
                .ThenBy(m => m.Title);
        }

    }
}
