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

namespace FWAMovies.Tests.Controllers.Fixtures
{
    [TestClass]
    public class GetMoviesByAverageRatingsTest : MovieControllerTestBase
    {
        Mock<IMovieContext> _movieContextMock;
        List<Movie> _movies;

        [TestInitialize]
        public void TestSetup()
        {
            _movieContextMock = new Mock<IMovieContext>();
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
        }

        [TestMethod]
        public void Get_Top_Five_Movies_By_Average_Ratings_Test()
        {
            //Arrange
            DbSet<Movie> movieDbSet = DbSetHelper.GetQueryableMockDbSet(_movies);
            _movieContextMock.Setup(m => m.Set<Movie>()).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Movies).Returns(movieDbSet);

            // prepare movies controller from  context
            MoviesController moviesController = BuildController(_movieContextMock.Object);
            List<int> expectedSequence = new List<int> { 6, 5, 4, 3, 2 };
            int expectedResultSize = 5;

            //Act
            IHttpActionResult response = moviesController.GetTopMovies();
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
        public void Top_Five_Movies_Where_Ratings_Draw_Test()
        {
            //Arrange
            _movies.Add(new Movie { ID = 9, Title = "Movie A", AverageRating = 3.768F });
            _movies.Add(new Movie { ID = 10, Title = "Movie B", AverageRating = 3.768F });

            DbSet<Movie> movieDbSet = DbSetHelper.GetQueryableMockDbSet(_movies);
            _movieContextMock.Setup(m => m.Set<Movie>()).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Movies).Returns(movieDbSet);

            // prepare movies controller from  context
            MoviesController moviesController = BuildController(_movieContextMock.Object);
            List<int> expectedSequence = new List<int> { 6, 9, 10, 5, 4 };
            int expectedResultSize = 5;

            //Act
            IHttpActionResult response = moviesController.GetTopMovies();
            var contentResult = response as OkNegotiatedContentResult<IEnumerable<MovieViewModel>>;
            List<MovieViewModel> actualMovies = contentResult.Content?.ToList();

            //Assert
            Assert.IsNotNull(actualMovies);
            Assert.AreEqual(actualMovies.Count, expectedResultSize);
            Assert.AreEqual(actualMovies.Select(s => s.Id).ToList().Intersect(expectedSequence).Count(), expectedResultSize);
        }

        /// <summary>
        /// When returning to an api consumer the average rating associated with a movie,
        /// round the number to the closest 0.5:
        /// </summary>
        /// <example>
        /// An average rating of 2.91 should be displayed as 3.0
        /// 3.249 should be displayed as 3.0;
        /// 3.75 should be displayed as 4.0.
        /// </example>
        [TestMethod]
        public void Get_Top_Five_Movies_Average_Rating_Round_Number_Test()
        {
            DbSet<Movie> movieDbSet = DbSetHelper.GetQueryableMockDbSet(_movies);
            _movieContextMock.Setup(m => m.Set<Movie>()).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Movies).Returns(movieDbSet);

            // prepare movies controller from  context
            MoviesController moviesController = BuildController(_movieContextMock.Object);
            List<double> expectedRoundNumbers = new List<double> { 4.0D, 4.0, 3.5, 3.5, 3.0 };
            int expectedResultSize = 5;

            //Act
            IHttpActionResult response = moviesController.GetTopMovies();
            var contentResult = response as OkNegotiatedContentResult<IEnumerable<MovieViewModel>>;
            List<MovieViewModel> actualMovies = contentResult.Content?.ToList();

            //Assert
            Assert.IsNotNull(actualMovies);
            Assert.AreEqual(actualMovies.Count, expectedResultSize);
            Assert.AreEqual(actualMovies.Select(s => s.AverageRating).ToList().Intersect(expectedRoundNumbers).Count(), 3);
        }
    }
}
