using FWAMovies.BusinessService.Interface;
using FWAMovies.Model;
using FWAMovies.ViewService.ViewModel;
using System.Collections.Generic;

namespace FWAMovies.ViewService.Service
{
    public class MovieViewService : IMovieViewService
    {
        private IMovieBusinessService _movieBusinessService;

        public MovieViewService(IMovieBusinessService movieBusinessService)
        {
            _movieBusinessService = movieBusinessService;
        }

        public IEnumerable<MovieViewModel> GetTopMovies()
        {
            IEnumerable<Movie> model = _movieBusinessService.GetTopMovies();

            return AutoMapper.Mapper.Map<IEnumerable<MovieViewModel>>(model);
        }
    }
}
