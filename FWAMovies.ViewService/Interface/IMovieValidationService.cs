using FWAMovies.ViewService.ViewModel;

namespace FWAMovies.ViewService.Interface
{
    public interface IMovieValidationService
    {
        bool IsValid(MovieFilterViewModel filter);
        bool IsValid(UserMovieReviewViewModel userMovieReview);
        bool IsValidUserMovie(UserMovieReviewViewModel userMovieReview);
    }
}
