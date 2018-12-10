using FWAMovies.Model;
using FWAMovies.Model.Dto;
using System.Collections.Generic;
using System.Linq;

namespace FWAMovies.DAL.Interface
{
    public interface IMovieRepository : IRepository<Movie>
    {
        /// <summary>
        /// Just a simple implementation to show class specific method.
        /// </summary>
        /// <returns><see cref="IQueryable<Movie>"/></returns>
        IQueryable<Movie> GetMovies();

        Movie GetById(int movieId);

        /// <summary>
        /// Just a simple implementation to show class specific method.
        /// </summary>
        /// <returns><see cref="IEnumerable<Movie>"/></returns>
        IEnumerable<Movie> GetTopMoviesByUserScore(int userId);

        /// <summary>
        /// Search list of movies based on search filter criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<Movie> GetMoviesBy(MovieFilter filter);
    }
}
