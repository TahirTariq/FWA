using FWAMovies.DAL.Interface;
using FWAMovies.Model;

namespace FWAMovies.DAL.Repository
{
    public class UserRepository : Repository<MovieContext, User>, IUserRepository
    {
        public UserRepository(MovieContext context) : base(context)
        {
        }
    }
}
