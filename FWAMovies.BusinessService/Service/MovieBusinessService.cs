﻿using FWAMovies.BusinessService.Extensions;
using FWAMovies.BusinessService.Interface;
using FWAMovies.DAL.Interface;
using FWAMovies.Model;
using FWAMovies.Model.Dto;
using System.Collections.Generic;
using System.Linq;

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
        /// Playing with my generic repository
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Movie> GetTopMoviesByUserScore(int userId)
        {
            return MovieRepository.Get
            (
                filter: movie => movie.Reviews.Any(r=> r.UserID == userId),
                orderBy: o => o.OrderByDescending(m => m.Reviews.FirstOrDefault().Rating)
                               .ThenBy(m => m.Title),
                includeProperties: "Reviews",
                take: 5
            );
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

            return MovieRepository.Get
            (
                filter: predicate,
                orderBy: o => o.OrderByDescending(m => m.AverageRating)
                               .ThenBy(m => m.Title)
            );
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
    }
}
