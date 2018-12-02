using FWAMovies.Model;
using System.Collections.Generic;

namespace FWAMovies.BusinessService.Interface
{
    public interface IMovieBusinessService
    {
        IEnumerable<Movie> GetTopMovies();
    }
}