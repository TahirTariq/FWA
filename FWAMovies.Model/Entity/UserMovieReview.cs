
namespace FWAMovies.Model
{
    public class UserMovieReview : IEntity
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int MovieID { get; set; }

        public float Rating { get; set; }
    }
}
