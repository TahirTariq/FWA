using FWAMovies.DAL.Context;
using FWAMovies.DAL.Interface;
using FWAMovies.Model;

namespace FWAMovies.DAL.Repository
{
    public class UserRepository : Repository<IMovieContext, User>, IUserRepository
    {
        public UserRepository(IMovieContext context) : base(context)
        {
        }
    }
}
