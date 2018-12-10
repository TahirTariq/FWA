using Microsoft.VisualStudio.TestTools.UnitTesting;
using FWAMovies.DAL.Interface;
using FWAMovies.Controllers;
using FWAMovies.DAL.Repository;
using FWAMovies.BusinessService.Service;
using FWAMovies.ViewService.Service;
using FWAMovies.ViewService;
using AutoMapper;
using FWAMovies.App_Start;

namespace FWAMovies.Tests.Controllers.Fixtures
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
}
