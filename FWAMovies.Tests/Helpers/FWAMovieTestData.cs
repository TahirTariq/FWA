using FWAMovies.Model;
using System;
using System.Collections.Generic;

namespace FWAMovies.Tests.Helpers
{
    public static class FWAMovieTestData
    {
        public static List<User> GetUsers()
        {
            var users = new List<User>
            {
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "1" },
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "2" },
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "3" },
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "4" },
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "5" },
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "6" },
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "7" },
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "8" },
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "9" },
               new User { ApiKey = new Guid(), FirstName = "User", LastName = "10" }
            };

            return users;
        }

        public static List<Movie> GetMovies()
        {
            var random = new Random();

            var movies = new List<Movie>
            {
                new Movie { ID = 1, Title = "Movie 1", AverageRating = 2.91F, Genres = "Horror, Thriller", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { ID = 2, Title = "Movie 2", AverageRating = 3.249F, Genres = "Action, Thriller", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { ID = 3, Title = "Movie 3", AverageRating = 3.25F, Genres = "Action, Comedy", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { ID = 4, Title = "Movie 4", AverageRating = 3.60F, Genres = "Horror, Comedy", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { ID = 5, Title = "Movie 5", AverageRating = 3.75F, Genres = "Horror, Romance", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { ID = 6, Title = "Movie 6", AverageRating = 3.81F, Genres = "Thriller, Romance", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { ID = 7, Title = "Movie 7", AverageRating = 2.31F, Genres = "Horror, Thriller", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { ID = 8, Title = "Movie 8", AverageRating = 1.63F, Genres = "Horror, Comedy", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
            };

            return movies;
        }


        public static List<UserMovieReview> GetUserMovieReviews()
        {
            var reviews = new List<UserMovieReview>()
            {
               new UserMovieReview{ UserID = 1, MovieID = 8, Rating = 3.60F},
               new UserMovieReview{ UserID = 1, MovieID = 7, Rating = 2.31F},
               new UserMovieReview{ UserID = 1, MovieID = 5, Rating = 3.60F},
               new UserMovieReview{ UserID = 1, MovieID = 3, Rating = 4.60F},
               new UserMovieReview{ UserID = 3, MovieID = 6, Rating = 1.60F},
               new UserMovieReview{ UserID = 5, MovieID = 4, Rating = 2.60F},
               new UserMovieReview{ UserID = 7, MovieID = 2, Rating = 3.50F},
               new UserMovieReview{ UserID = 8, MovieID = 1, Rating = 3.90F},
               new UserMovieReview{ UserID = 9, MovieID = 8, Rating = 1.40F},
            };

            return reviews;
        }
    }
}
