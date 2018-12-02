using System;
using System.Collections.Generic;

namespace FWAMovies.Model
{
    public class User : IEntity
    {
        public int ID { get; set; }

        public Guid ApiKey { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<UserMovieReview> Reviews { get; set; }
    }
}
