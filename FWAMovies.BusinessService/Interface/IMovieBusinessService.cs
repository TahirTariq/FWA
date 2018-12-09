using FWAMovies.Model;
using FWAMovies.Model.Dto;
using System.Collections.Generic;

namespace FWAMovies.BusinessService.Interface
{
    public interface IMovieBusinessService
    {
        User GetUserById(int userId);

        Movie GetMovieById(int movieId);

        IEnumerable<Movie> GetTopMovies();

        IEnumerable<Movie> GetTopMoviesByUserScore(int userId);

        IEnumerable<Movie> GetMoviesBy(MovieFilter filter);

        UserMovieReview SubmitUserMovieReview(UserMovieReview userReview);
    }
}