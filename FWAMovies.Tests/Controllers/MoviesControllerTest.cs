﻿using System.Linq;
using System.Collections.Generic;
using FWAMovies.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FWAMovies.DAL.Interface;
using FWAMovies.Tests.Helpers;
using FWAMovies.Controllers;
using FWAMovies.DAL.Repository;
using FWAMovies.BusinessService.Service;
using FWAMovies.ViewService.Service;
using FWAMovies.ViewService;
using AutoMapper;
using FWAMovies.App_Start;
using System.Data.Entity;
using Moq;
using System.Web.Http;
using System.Web.Http.Results;
using FWAMovies.ViewService.ViewModel;
using System;

namespace FWAMovies.Tests.Controllers
{
    public class MoviesControllerTest
    {
        [TestClass]
        public class MovieControllerTestBase
        {
            protected virtual MoviesController BuildController(IMovieContext movieContext)
            {
                IMovieRepository movieRepository = new MovieRepository(movieContext);
                IUserRepository userRepository = new UserRepository(movieContext);
                IUserMovieReviewRepository movieReviewRepository = new UserMovieReviewRepository(movieContext);

                MovieBusinessService movieBusiness = new MovieBusinessService
                (
                   movieRepository,
                   userRepository,
                   movieReviewRepository
                );

                MovieValidationService validationService = new MovieValidationService
                (
                   movieBusiness
                );

                IMovieViewService movieViewService = new MovieViewService(movieBusiness);

                var moviesController = new MoviesController
                (
                    movieViewService,
                    validationService
                );

                return moviesController;
            }

            [AssemblyInitialize]
            public static void AssemblyInit(TestContext context)
            {
                // Only should initialize once per fixtur execution.
                Mapper.Initialize(AutoMapperRegisterProfiles.RegisterProfiles);
            }
        }

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
                List<double> expectedRoundNumbers= new List<double> { 4.0D, 4.0, 3.5, 3.5, 3.0 };
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
                IHttpActionResult response = moviesController.UserReview(new UserMovieReviewViewModel { UserID = 1, MovieID = 1, Rating =5 });
                var contentResult = response as OkResult;
                var context = _movieContextMock.Object;
                
                Assert.IsNotNull(contentResult);
                float actualRating = context.UserMovieReviews.FirstOrDefault(r => r.MovieID == 1 && r.UserID == 1).Rating;
                Assert.AreEqual(expectedRating, actualRating);
            }
        }

        // TODO
        [TestClass]
        public class GetMoviesByFilterTest
        {
            [TestMethod]
            public void No_Filter_Criteria_Test()
            {

            }

            [TestMethod]
            public void Title_Criteria_Test()
            {

            }

            [TestMethod]
            public void Genre_Criteria_Test()
            {

            }

            [TestMethod]
            public void Combined_Criteria_Test()
            {

            }

            [TestMethod]
            public void Combined_Criteria_Average_Rating_Round_Test()
            {

            }

        }
    }
}
