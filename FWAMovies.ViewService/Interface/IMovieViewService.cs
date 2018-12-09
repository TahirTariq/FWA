using FWAMovies.ViewService.ViewModel;
using System.Collections.Generic;

namespace FWAMovies.ViewService
{
    public interface IMovieViewService
    {
        IEnumerable<MovieViewModel> GetTopMovies();

        IEnumerable<MovieViewModel> GetTopMoviesByUserScore(int userId);

        IEnumerable<MovieViewModel> GetMoviesBy(MovieFilterViewModel filter);

        UserMovieReviewViewModel SubmitUserMovieReview(UserMovieReviewViewModel userReview);
    }
}
