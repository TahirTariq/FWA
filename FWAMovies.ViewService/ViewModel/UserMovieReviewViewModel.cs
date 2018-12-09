using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWAMovies.ViewService.ViewModel
{
    public class UserMovieReviewViewModel
    {
        public int UserID { get; set; }

        public int MovieID { get; set; }

        public float Rating { get; set; }
    }
}
