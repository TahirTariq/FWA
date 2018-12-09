
using FWAMovies.BusinessService.Interface;
using FWAMovies.ViewService.Interface;
using FWAMovies.ViewService.ViewModel;

namespace FWAMovies.ViewService.Service
{
    /// <summary>
    /// Ideally this validation class should be replaced with FluentValidation
    /// but just to save time I am using custom validation service class.
    /// </summary>
    public class MovieValidationService : IMovieValidationService
    {
        public MovieValidationService(IMovieBusinessService movieBusinessService)
        {
            MovieBusinessService = movieBusinessService;
        }

        protected IMovieBusinessService MovieBusinessService { get; }

        public bool IsValid(MovieFilterViewModel filter)
        {
            if (filter == null) return false;

            bool isValid = !string.IsNullOrWhiteSpace(filter.Title) ||
                filter.YearOfRelease != null ||
                (filter.Genre != null && filter.Genre.Length > 0);

            return isValid;
        }

        public bool IsValid(UserMovieReviewViewModel userMovieReview)
        {
            if (userMovieReview == null) return false;

            return userMovieReview.Rating <= 5 && userMovieReview.Rating >= 0;
        }

        public bool IsValidUserMovie(UserMovieReviewViewModel userMovieReview)
        {
            return IsValidMovieId(userMovieReview.MovieID)
                || IsValidUserId(userMovieReview.UserID);
        }

        private bool IsValidMovieId(int movieId) =>
            MovieBusinessService.GetMovieById(movieId) != null;

        private bool IsValidUserId(int userId) =>
            MovieBusinessService.GetUserById(userId) != null;
    }
}
