
using System.Collections.Generic;

namespace FWAMovies.Model
{
    public class Movie : IEntity
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public int YearOfRelease { get; set; }

        public int RunningTime { get; set; }

        public float AverageRating { get; set; }

        // Genre in CSV string - to simplify
        public string Genres { get; set; }

        public virtual ICollection<UserMovieReview> Reviews { get; set; }
    }
}
