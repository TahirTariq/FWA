using FWAMovies.BusinessService.Interface;
using FWAMovies.DAL.Interface;
using FWAMovies.Model;
using FWAMovies.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FWAMovies.BusinessService.Service
{
    public class MovieBusinessService : IMovieBusinessService
    {
        public MovieBusinessService
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
            return MovieRepository.Get
            (
                orderBy: f => f.OrderByDescending(m => m.AverageRating)
                               .ThenBy(m => m.Title),
                take: 5
            );
        }

        /// <summary>
        /// Playing with my generic repository,  it's not ready 
        /// yet so reverting to classic repository methods
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Movie> GetTopMoviesByUserScore_Practice(int userId)
        {
            return MovieRepository.Get
            (
                filter: movie => movie.Reviews.Any(r=> r.UserID == userId),
                orderBy: o => o.OrderByDescending(m => m.Reviews.First().Rating)
                               .ThenBy(m => m.Title),
                includeProperties: "Reviews",
                take: 5
            );
        }

        public IEnumerable<Movie> GetTopMoviesByUserScore(int userId)
        {
            return MovieRepository.GetTopMoviesByUserScore(userId);
        }

        public IEnumerable<Movie> GetMoviesBy(MovieFilter filter)
        {
            //Expression<Func<Movie, bool>> filterBy = m => true;
            Func<Movie, bool> filterBy = (m) =>
            {
                bool result = false;

                if (!string.IsNullOrWhiteSpace(filter.Title))
                {
                    result = m.Title == filter.Title;
                }

                if (filter.YearOfRelease != null)
                {
                    result = result || m.YearOfRelease == filter.YearOfRelease;
                }

                if (filter.Genre != null && filter.Genre.Length>0)
                {
                    foreach (string genre in filter.Genre)
                    {
                        result = result || m.Genres.Contains(genre);
                    }
                }

                return result;
            };

            Expression<Func<Movie, bool>> filterByExp = FuncToExpression(filterBy);

            //List<Expression<Func<Movie, bool>>> expressions = new List<Expression<Func<Movie, bool>>>();

            //if (!string.IsNullOrWhiteSpace(filter.Title))
            //{
            //    Expression<Func<Movie, bool>> titleEx = movie => movie.Title == filter.Title;

            //    expressions.Add(titleEx);
            //}

            //Expression<Func<Movie, bool>> filterBy1 = Expression.Block(expressions);
            return MovieRepository.Get
            (
                filter: filterByExp,
                orderBy: o => o.OrderByDescending(m => m.AverageRating)
                               .ThenBy(m => m.Title),
                take: 5
            );
        }

        static Expression<Func<T, TResult>> FuncToExpression<T, TResult>(Func<T, TResult> method)
        {
            return x => method(x);
        }

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

        // Alternate implementation
        public IEnumerable<Movie> GetTopMoviesAlternate()
        {
            IEnumerable<Movie> result = MovieRepository
                  .GetMovies()
                  .OrderByDescending(m => m.AverageRating)
                  .ThenBy(m=> m.Title)
                  .Take(5);

            return result;
        }
    }
}
