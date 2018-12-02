using AutoMapper;
using FWAMovies.Model;
using FWAMovies.ViewService.ViewModel;
using System;

namespace FWAMovies.ViewService.AutoMapperProfiles
{
    public class MovieModelProfile : Profile
    {
        public MovieModelProfile()
        {
            CreateMap<Movie, MovieViewModel>()
                  .ForMember(dest => dest.Id,
                       opts => opts.MapFrom(src => src.ID))
                  .ForMember(dest => dest.AverageRating,
                        opts => opts.MapFrom(src => Math.Round(src.AverageRating)));
        }
    }
}
