using AutoMapper;
using FWAMovies.App_Start;
using System.Web.Http;

namespace FWAMovies
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(FormattersConfig.Register);
            GlobalConfiguration.Configure(UnityContainerConfig.Register);
            Mapper.Initialize(AutoMapperRegisterProfiles.RegisterProfiles);
        }
    }
}
