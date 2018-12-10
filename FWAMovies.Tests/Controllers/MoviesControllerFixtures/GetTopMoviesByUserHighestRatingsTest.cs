using System.Linq;
using System.Collections.Generic;
using FWAMovies.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FWAMovies.DAL.Interface;
using FWAMovies.Tests.Helpers;
using FWAMovies.Controllers;
using System.Data.Entity;
using Moq;
using System.Web.Http;
using System.Web.Http.Results;
using FWAMovies.ViewService.ViewModel;
using System;


namespace FWAMovies.Tests.Controllers.Fixtures
{
    [TestClass]
    public class GetTopMoviesByUserHighestRatingsTest : MovieControllerTestBase
    {
        Mock<IMovieContext> _movieContextMock;
        List<Movie> _movies;

        [TestInitialize]
        public void TestSetup()
        {
            _movieContextMock = new Mock<IMovieContext>();
            var reviews = new List<UserMovieReview>()
                {
                   new UserMovieReview{ UserID = 1, MovieID = 8, Rating = 1.60F},
                   new UserMovieReview{ UserID = 1, MovieID = 7, Rating = 3.60F},
                   new UserMovieReview{ UserID = 1, MovieID = 5, Rating = 3.60F},
                   new UserMovieReview{ UserID = 1, MovieID = 3, Rating = 4.60F},
                   new UserMovieReview{ UserID = 3, MovieID = 6, Rating = 1.60F},
                   new UserMovieReview{ UserID = 5, MovieID = 4, Rating = 2.60F},
                   new UserMovieReview{ UserID = 7, MovieID = 2, Rating = 3.50F},
                   new UserMovieReview{ UserID = 8, MovieID = 1, Rating = 3.90F},
                   new UserMovieReview{ UserID = 9, MovieID = 8, Rating = 1.40F}
                };

            _movies = new List<Movie>
                {
                    new Movie { ID = 1, Title = "Movie 1", AverageRating = 2.91F},
                    new Movie { ID = 2, Title = "Movie 2", AverageRating = 3.249F},
                    new Movie { ID = 3, Title = "Movie 3", AverageRating = 3.25F},
                    new Movie { ID = 4, Title = "Movie 4", AverageRating = 3.60F},
                    new Movie { ID = 5, Title = "Movie 5", AverageRating = 3.75F},
                    new Movie { ID = 6, Title = "Movie 6", AverageRating = 3.81F},
                    new Movie { ID = 7, Title = "Movie 7", AverageRating = 2.31F},
                    new Movie { ID = 8, Title = "Movie 8", AverageRating = 1.63F}
                };

            // Setup relationship

            Func<int, ICollection<UserMovieReview>> GetMovieReviews =
                (movieId) => reviews.Where(r => r.MovieID == movieId).ToList();

            Func<int, Movie> GetMovie =
               (movieId) => _movies.Where(r => r.ID == movieId).FirstOrDefault();

            reviews.ForEach(r => r.ReviewMovie = GetMovie(r.MovieID));

            _movies.ForEach(m => m.Reviews = GetMovieReviews(m.ID));
        }


        [TestMethod]
        public void Get_Top_Five_Movies_By_User_Score_Test()
        {
            //Arrange
            DbSet<Movie> movieDbSet = DbSetHelper.GetQueryableMockDbSet(_movies);
            DbSet<UserMovieReview> userMovieReviewDbSet = DbSetHelper.GetQueryableMockDbSet(FWAMovieTestData.GetUserMovieReviews());
            _movieContextMock.Setup(m => m.Set<Movie>()).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Movies).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Set<UserMovieReview>()).Returns(userMovieReviewDbSet);
            _movieContextMock.Setup(m => m.UserMovieReviews).Returns(userMovieReviewDbSet);

            // prepare movies controller from  context
            MoviesController moviesController = BuildController(_movieContextMock.Object);
            List<int> expectedSequence = new List<int> { 3, 5, 7, 8 };
            int expectedResultSize = 4;

            //Act
            IHttpActionResult response = moviesController.GetTopMoviesByUser(1);
            var contentResult = response as OkNegotiatedContentResult<IEnumerable<MovieViewModel>>;
            List<MovieViewModel> actualMovies = contentResult.Content?.ToList();

            //Assert
            Assert.IsNotNull(actualMovies);
            Assert.AreEqual(actualMovies.Count, expectedResultSize);
            Assert.AreEqual(actualMovies.Select(s => s.Id).ToList().Intersect(expectedSequence).Count(), expectedResultSize);
        }

        /// <summary>
        /// In case of a rating draw, (e.g. 2 movies have 3.768 average rating) 
        /// return them by ascending  title alphabetical order.
        /// </summary>
        [TestMethod]
        public void Get_Top_Five_Movies_Where_User_Score_Draw_Test()
        {
            //Arrange
            DbSet<Movie> movieDbSet = DbSetHelper.GetQueryableMockDbSet(_movies);
            DbSet<UserMovieReview> userMovieReviewDbSet = DbSetHelper.GetQueryableMockDbSet(FWAMovieTestData.GetUserMovieReviews());
            _movieContextMock.Setup(m => m.Set<Movie>()).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Movies).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Set<UserMovieReview>()).Returns(userMovieReviewDbSet);
            _movieContextMock.Setup(m => m.UserMovieReviews).Returns(userMovieReviewDbSet);

            // prepare movies controller from  context
            MoviesController moviesController = BuildController(_movieContextMock.Object);
            List<int> expectedSequence = new List<int> { 3, 5, 7, 8 };
            int expectedResultSize = 4;

            //Act
            IHttpActionResult response = moviesController.GetTopMoviesByUser(1);
            var contentResult = response as OkNegotiatedContentResult<IEnumerable<MovieViewModel>>;
            List<MovieViewModel> actualMovies = contentResult.Content?.ToList();

            //Assert
            Assert.IsNotNull(actualMovies);
            Assert.AreEqual(actualMovies.Count, expectedResultSize);
            Assert.AreEqual(actualMovies.Select(s => s.Id).ToList().Intersect(expectedSequence).Count(), expectedResultSize);
        }
    }

}
