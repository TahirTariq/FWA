using FWAMovies.Resolver;
using FWAMovies.ViewService;
using FWAMovies.ViewService.AutoMapperProfiles;
using FWAMovies.ViewService.Service;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace FWAMovies
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            InitializeAutoMapper();

            ConfigureDependencyResolver(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void InitializeAutoMapper()
        {
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<MovieModelProfile>());
        }

        public static void ConfigureDependencyResolver(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IMovieViewService, MovieViewService>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}
