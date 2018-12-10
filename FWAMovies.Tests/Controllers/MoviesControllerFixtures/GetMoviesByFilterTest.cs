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
    public class GetMoviesByFilterTest : MovieControllerTestBase
    {
        Mock<IMovieContext> _movieContextMock;

        [TestInitialize]
        public void TestSetup()
        {
            _movieContextMock = new Mock<IMovieContext>();
        }

        [TestMethod]
        public void No_Filter_Criteria_Test()
        {
            // Arrange
            DbSet<Movie> movieDbSet = DbSetHelper.GetQueryableMockDbSet(
                new List<Movie> {
                        new Movie { ID = 1, Reviews = new List<UserMovieReview>() }
                }
            );

            _movieContextMock.Setup(m => m.Set<Movie>()).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Movies).Returns(movieDbSet);

            DbSet<UserMovieReview> userMovieReviewDbSet = DbSetHelper.GetQueryableMockDbSet(new List<UserMovieReview>());
            _movieContextMock.Setup(m => m.Set<UserMovieReview>()).Returns(userMovieReviewDbSet);
            _movieContextMock.Setup(m => m.UserMovieReviews).Returns(userMovieReviewDbSet);

            DbSet<User> userDbSet = DbSetHelper.GetQueryableMockDbSet(new List<User> { new User { ID = 1 } });
            _movieContextMock.Setup(m => m.Set<User>()).Returns(userDbSet);
            _movieContextMock.Setup(m => m.Users).Returns(userDbSet);

            MoviesController moviesController = BuildController(_movieContextMock.Object);

            // Act
            IHttpActionResult response = moviesController.GetMoviesBy(null);
            var contentResult = response as BadRequestErrorMessageResult;

            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void Title_Criteria_Test()
        {
            // Arrange
            DbSet<Movie> movieDbSet = DbSetHelper.GetQueryableMockDbSet(
                new List<Movie> {
                        new Movie { ID = 1, Title= "A", Reviews = new List<UserMovieReview>() }
                }
            );

            _movieContextMock.Setup(m => m.Set<Movie>()).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Movies).Returns(movieDbSet);

            DbSet<UserMovieReview> userMovieReviewDbSet = DbSetHelper.GetQueryableMockDbSet(new List<UserMovieReview>());
            _movieContextMock.Setup(m => m.Set<UserMovieReview>()).Returns(userMovieReviewDbSet);
            _movieContextMock.Setup(m => m.UserMovieReviews).Returns(userMovieReviewDbSet);

            DbSet<User> userDbSet = DbSetHelper.GetQueryableMockDbSet(new List<User> { new User { ID = 1 } });
            _movieContextMock.Setup(m => m.Set<User>()).Returns(userDbSet);
            _movieContextMock.Setup(m => m.Users).Returns(userDbSet);

            MoviesController moviesController = BuildController(_movieContextMock.Object);

            // Act
            IHttpActionResult response = moviesController.GetMoviesBy(new MovieFilterViewModel { Title = "a" });
            var contentResult = response as OkNegotiatedContentResult<IEnumerable<MovieViewModel>>;


            Assert.IsNotNull(contentResult);
            Assert.AreEqual(contentResult.Content.Count(), 1);
        }
    }
}
