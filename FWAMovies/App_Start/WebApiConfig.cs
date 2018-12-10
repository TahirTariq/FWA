using System.Web.Http;

namespace FWAMovies
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

        }
    }
}
