using FWAMovies.ViewService.ViewModel;
using System.Collections.Generic;

namespace FWAMovies.ViewService
{
    public interface IMovieViewService
    {
        IEnumerable<MovieViewModel> GetTopMovies();
    }
}
