using FWAMovies.BusinessService.Interface;
using FWAMovies.BusinessService.Service;
using FWAMovies.Controllers;
using FWAMovies.DAL.Context;
using FWAMovies.DAL.Interface;
using FWAMovies.DAL.Repository;
using FWAMovies.Resolver;
using FWAMovies.ViewService;
using FWAMovies.ViewService.Interface;
using FWAMovies.ViewService.Service;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace FWAMovies.App_Start
{
    public class UnityContainerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            RegisterDependency(container);
            config.DependencyResolver = new UnityResolver(container);
        }

        public static void RegisterDependency(UnityContainer container)
        {
            container.RegisterType<IMovieContext, MovieContext>(new InjectionConstructor("FWAMoviesDbConnectionString"));
            container.RegisterType<IMovieRepository, MovieRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserMovieReviewRepository, UserMovieReviewRepository>();
            container.RegisterType<IMovieBusinessService, MovieBusinessService>(new HierarchicalLifetimeManager());

            //container.RegisterType<IMovieBusinessService, MovieBusinessServiceAlternate>(new HierarchicalLifetimeManager());

            container.RegisterType<IMovieValidationService, MovieValidationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMovieViewService, MovieViewService>(new HierarchicalLifetimeManager());
            container.RegisterType<MoviesController>(new HierarchicalLifetimeManager());
        }
    }
}