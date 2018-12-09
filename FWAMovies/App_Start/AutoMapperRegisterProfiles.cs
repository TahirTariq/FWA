using AutoMapper;
using FWAMovies.ViewService.AutoMapperProfiles;

namespace FWAMovies.App_Start
{
    public static class AutoMapperRegisterProfiles
    {
        public static void RegisterProfiles(IMapperConfigurationExpression config)
        {
            config.AddProfile<MovieModelProfile>();           
        }
    }
}