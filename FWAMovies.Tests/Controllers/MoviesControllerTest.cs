using System.Linq;
using System.Collections.Generic;
using FWAMovies.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using FWAMovies.DAL.Interface;
using FWAMovies.BusinessService.Service;
using FWAMovies.Controllers;

namespace FWAMovies.Tests.Controllers
{
    public class MoviesControllerTest
    {
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

        [TestClass]
        public class GetMoviesByAverageRatingsTest
        {
            private IMovieRepository _mockDao;
            private MovieBusinessService _mockBusiness;
            private MoviesController _moviesController;

            [TestInitialize]
            public void SetupTest()
            {
                _mockDao = MockRepository.GenerateMock<IMovieRepository>();
                _mockBusiness = MockRepository.GenerateMock<MovieBusinessService>(_mockDao);
                _moviesController = new MoviesController(_mockBusiness);
            }

            [TestMethod]
            public void Top_Five_Movies_By_Average_Ratings_Test()
            {
                // Arrange

                var movies = new List<Movie>
                {
                    new Movie { ID = 1, Title = "A", AverageRating = 9 },
                     new Movie { ID = 2, Title = "C", AverageRating = 8 },
                      new Movie { ID = 3, Title = "D", AverageRating = 7 },
                       new Movie { ID = 4, Title = "E", AverageRating = 7 },
                        new Movie { ID = 5, Title = "F", AverageRating = 5 },
                         new Movie { ID = 6, Title = "B", AverageRating = 8 }
                };

                _mockDao.Stub(dao => dao.GetMovies()).IgnoreArguments().Repeat.Any().Return(movies.AsQueryable());


                // Act
                List<Movie> result = _moviesController.GetTopMovies().ToList();

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(5, result.Count());
                Assert.AreEqual(1, result.ElementAt(0).ID);
                Assert.AreEqual(6, result.ElementAt(1).ID);
            }

            [TestMethod]
            public void Top_Five_Movies_Where_Ratings_Draw_Test()
            {
                // In case of a rating draw, (e.g. 2 movies have 3.768 average rating) return them by ascending
                //title alphabetical order.


            }

            [TestMethod]
            public void Top_Five_Movies_Average_Rating_Round_Test()
            {
                // In case of a rating draw, (e.g. 2 movies have 3.768 average rating) return them by ascending
                //title alphabetical order.


            }


        }

        [TestClass]
        public class GetTopMoviesByUserHighestRatingsTest
        {
            [TestMethod]
            public void Top_Five_Movies_By_User_Score_Test()
            {

            }

            [TestMethod]
            public void Top_Five_Movies_Where_User_Score_Draw_Test()
            {
                // In case of a rating draw, (e.g. 2 movies have 3.768 average rating) return them by ascending
                //title alphabetical order.


            }

            [TestMethod]
            public void Top_Five_Movies_By_User_Score_Rating_Round_Test()
            {
                // In case of a rating draw, (e.g. 2 movies have 3.768 average rating) return them by ascending
                //title alphabetical order.


            }
        }

        [TestClass]
        public class SubmitUserRating
        {
            [TestMethod]
            public void New_Movie_Rating_Test()
            {

            }

            [TestMethod]
            public void Update_Existing_Movie_Rating_Test()
            {
                // In case of a rating draw, (e.g. 2 movies have 3.768 average rating) return them by ascending
                //title alphabetical order.


            }
        }
    }
}
