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
    public class SubmitUserRating : MovieControllerTestBase
    {
        Mock<IMovieContext> _movieContextMock;

        [TestInitialize]
        public void TestSetup()
        {
            _movieContextMock = new Mock<IMovieContext>();
        }

        [TestMethod]
        public void New_Movie_Rating_Test()
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
            IHttpActionResult response = moviesController.UserReview(new UserMovieReviewViewModel { UserID = 1, MovieID = 1 });
            var contentResult = response as OkResult;

            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void Update_Existing_Movie_Rating_Test()
        {
            // Arrange

            DbSet<Movie> movieDbSet = DbSetHelper.GetQueryableMockDbSet(
                new List<Movie> {
                        new Movie
                        {
                            ID = 1, Reviews = new List<UserMovieReview> { new UserMovieReview { MovieID = 1,UserID = 1, Rating = 4 } }
                        }
                }
            );

            _movieContextMock.Setup(m => m.Set<Movie>()).Returns(movieDbSet);
            _movieContextMock.Setup(m => m.Movies).Returns(movieDbSet);

            DbSet<UserMovieReview> userMovieReviewDbSet = DbSetHelper.GetQueryableMockDbSet(new List<UserMovieReview> { new UserMovieReview { MovieID = 1, UserID = 1, Rating = 4 } });
            _movieContextMock.Setup(m => m.Set<UserMovieReview>()).Returns(userMovieReviewDbSet);
            _movieContextMock.Setup(m => m.UserMovieReviews).Returns(userMovieReviewDbSet);

            DbSet<User> userDbSet = DbSetHelper.GetQueryableMockDbSet(new List<User> { new User { ID = 1 } });
            _movieContextMock.Setup(m => m.Set<User>()).Returns(userDbSet);
            _movieContextMock.Setup(m => m.Users).Returns(userDbSet);

            MoviesController moviesController = BuildController(_movieContextMock.Object);
            float expectedRating = 5;

            // Act
            IHttpActionResult response = moviesController.UserReview(new UserMovieReviewViewModel { UserID = 1, MovieID = 1, Rating = 5 });
            var contentResult = response as OkResult;
            var context = _movieContextMock.Object;

            Assert.IsNotNull(contentResult);
            float actualRating = context.UserMovieReviews.FirstOrDefault(r => r.MovieID == 1 && r.UserID == 1).Rating;
            Assert.AreEqual(expectedRating, actualRating);
        }
    }
}
