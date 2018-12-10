using FWAMovies.BusinessService.Interface;
using FWAMovies.DAL.Interface;
using FWAMovies.Model;
using FWAMovies.Model.Dto;
using System.Collections.Generic;
using System.Linq;

namespace FWAMovies.BusinessService.Service
{
    /// <summary>
    /// This is an alternate implementatin of <see cref="MovieBusinessService"/>
    /// that instead uses specific repository methods than generic.
    /// </summary>
    public class MovieBusinessServiceAlternate : IMovieBusinessService
    {
        public MovieBusinessServiceAlternate
        (
            IMovieRepository movieRepositoryArg,
            IUserRepository userRepository,
            IUserMovieReviewRepository userMovieReviewRepository
        )
        {
            MovieRepository = movieRepositoryArg;
            UserRepository = userRepository;
            UserMovieReviewRepository = userMovieReviewRepository;
        }

        protected IMovieRepository MovieRepository { get; }

        protected IUserRepository UserRepository { get; }

        protected IUserMovieReviewRepository UserMovieReviewRepository { get; }

        // TODO: should go in UserBusinessService
        public User GetUserById(int userId)
        {
            return UserRepository.GetById(userId);
        }

        public Movie GetMovieById(int movieId)
        {
            return MovieRepository.GetById(movieId);
        }

        public IEnumerable<Movie> GetTopMovies()
        {
            IEnumerable<Movie> result = MovieRepository
                  .GetMovies()
                  .OrderByDescending(m => m.AverageRating)
                  .ThenBy(m => m.Title)
                  .Take(5);

            return result;
        }

        public IEnumerable<Movie> GetTopMoviesByUserScore(int userId)
        {
            return MovieRepository.GetTopMoviesByUserScore(userId);
        }

        public IEnumerable<Movie> GetMoviesBy(MovieFilter filter)
        {
            return MovieRepository.GetMoviesBy(filter);
        }

        /// <summary>
        /// TODO: need to convert into specific repository calls.
        /// </summary>
        /// <param name="userReview"></param>
        /// <returns></returns>
        public UserMovieReview SubmitUserMovieReview(UserMovieReview userReview)
        {
            var existingReview = UserMovieReviewRepository.Get(
                filter: f => f.UserID == userReview.UserID &&
                            f.MovieID == userReview.MovieID).FirstOrDefault();

            Movie movie = MovieRepository.GetById(userReview.MovieID);
            movie.AverageRating = CalculateMovieRatingAverage(movie, existingReview, userReview);

            if (existingReview == null)
            {
                UserMovieReview newReview = new UserMovieReview
                {
                    UserID = userReview.UserID,
                    MovieID = userReview.MovieID,
                    Rating = userReview.Rating
                };

                UserMovieReviewRepository.Create(newReview);
            }
            else
            {
                existingReview.Rating = userReview.Rating;
            }

            var isEq = MovieRepository.Context().Equals(UserMovieReviewRepository.Context());

            var e = MovieRepository.Context() == UserMovieReviewRepository.Context();
            MovieRepository.Save();
            UserMovieReviewRepository.Save();

            return existingReview;
        }

        private float CalculateMovieRatingAverage(Movie movie, UserMovieReview existingReview, UserMovieReview userReview)
        {
            bool newUser = existingReview == null;

            int numberOfUsersReviews = movie.Reviews.Count();

            float totalScore = numberOfUsersReviews * movie.AverageRating;

            if (newUser)
            { numberOfUsersReviews++; }
            else
            { totalScore -= existingReview.Rating; }

            totalScore += userReview.Rating;

            float newScore = totalScore / numberOfUsersReviews;

            return newScore;
        }   
    }
}
