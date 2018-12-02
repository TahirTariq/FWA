using FWAMovies.ViewService;
using FWAMovies.ViewService.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace FWAMovies.Controllers
{
    public class MoviesController : ApiController
    {
        private IMovieViewService _movieViewService;

        public MoviesController(IMovieViewService movieViewService)
        {
            _movieViewService = movieViewService;
        }

        public IEnumerable<MovieViewModel> GetTopMovies()
        {
            return _movieViewService.GetTopMovies();    
        }
    }
}
