using FWAMovies.BusinessService.Interface;
using FWAMovies.DAL.Interface;
using FWAMovies.Model;
using System.Collections.Generic;
using System.Linq;

namespace FWAMovies.BusinessService.Service
{
    public class MovieBusinessService : IMovieBusinessService
    {
        protected IMovieRepository movieRepository;

        public MovieBusinessService(IMovieRepository movieRepositoryArg)
        {
            movieRepository = movieRepositoryArg;
        }

        // Alternate implementation
        public IEnumerable<Movie> GetTopMoviesAlternate()
        {
            return movieRepository.Get(
                orderBy: f => f.OrderByDescending(m => m.AverageRating)
                               .ThenBy(m => m.Title),
                take: 5);
        }

        public IEnumerable<Movie> GetTopMovies()
        {
            var movies = movieRepository.GetMovies();

            IEnumerable<Movie> result = movieRepository
                  .GetMovies()
                  .OrderByDescending(m => m.AverageRating)
                  .OrderBy(m => m.Title)
                  .Take(5);

            return result;
        }
    }
}
