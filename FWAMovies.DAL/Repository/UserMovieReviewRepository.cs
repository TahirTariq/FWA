using FWAMovies.DAL.Context;
using FWAMovies.DAL.Interface;
using FWAMovies.Model;

namespace FWAMovies.DAL.Repository
{
    public class UserMovieReviewRepository : Repository<IMovieContext, UserMovieReview>, IUserMovieReviewRepository
    {
        public UserMovieReviewRepository(IMovieContext context) : base(context)
        {
        }
    }
}
