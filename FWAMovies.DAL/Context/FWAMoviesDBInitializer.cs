using FWAMovies.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace FWAMovies.DAL.Context
{
    public class FWAMoviesDBInitializer : DropCreateDatabaseAlways<MovieContext>
    {
        protected override void Seed(MovieContext context)
        {
            var random = new Random();

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

            var movies = new List<Movie>
            {
                new Movie { Title = "Movie 1", Genres = "Horror, Thriller", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { Title = "Movie 2", Genres = "Action, Thriller", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { Title = "Movie 3", Genres = "Action, Comedy", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { Title = "Movie 4", Genres = "Horror, Comedy", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { Title = "Movie 5", Genres = "Horror, Romance", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { Title = "Movie 6", Genres = "Thriller, Romance", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { Title = "Movie 7", Genres = "Horror, Thriller", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },
                new Movie { Title = "Movie 8", Genres = "Horror, Comedy", RunningTime = random.Next(150, 400), YearOfRelease = random.Next(1900, 2020)  },

            };

            Func<float> randomRating = () => (float)(random.Next(0, 10) * random.NextDouble());

            var reviews = new List<UserMovieReview>()
            {
               new UserMovieReview{ UserID = 1, MovieID = 8, Rating = randomRating()},
               new UserMovieReview{ UserID = 2, MovieID = 7, Rating = randomRating()},
               new UserMovieReview{ UserID = 3, MovieID = 6, Rating = randomRating()},
               new UserMovieReview{ UserID = 4, MovieID = 5, Rating = randomRating()},
               new UserMovieReview{ UserID = 5, MovieID = 4, Rating = randomRating()},
               new UserMovieReview{ UserID = 6, MovieID = 3, Rating = randomRating()},
               new UserMovieReview{ UserID = 7, MovieID = 2, Rating = randomRating()},
               new UserMovieReview{ UserID = 8, MovieID = 1, Rating = randomRating()},
               new UserMovieReview{ UserID = 9, MovieID = 8, Rating = randomRating()},
            };

            context.Users.AddRange(users);

            context.Movies.AddRange(movies);

            context.UserMovieReviews.AddRange(reviews);

            base.Seed(context);
        }
    }
}
