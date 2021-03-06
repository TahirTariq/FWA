﻿using FWAMovies.ViewService;
using FWAMovies.ViewService.Interface;
using FWAMovies.ViewService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FWAMovies.Controllers
{

    public class MoviesController : ApiController
    {
        public MoviesController
        (
            IMovieViewService movieViewService,
            IMovieValidationService movieValidationService
        )
        {
            MovieViewService = movieViewService;
            MovieValidationService = movieValidationService;
        }

        protected IMovieViewService MovieViewService { get; }
        protected IMovieValidationService MovieValidationService { get; }

        [Route("api/movies/filter")]
        [HttpGet]
        public IHttpActionResult GetMoviesBy([FromUri] MovieFilterViewModel filter)
        {
            bool isModelValid = MovieValidationService.IsValid(filter);

            if (!isModelValid)
            {
                return BadRequest("Invalid / no criteria is give");
            }

            IEnumerable<MovieViewModel> filterResult = MovieViewService.GetMoviesBy(filter);

            if (IsNullOrEmpty(filterResult))
                return NotFound();

            return Ok(filterResult);
        }

        [Route("api/movies/top")]
        [Route("api/movies")]
        [HttpGet]
        public IHttpActionResult GetTopMovies()
        {
            IEnumerable<MovieViewModel> result = MovieViewService.GetTopMovies();

            if (IsNullOrEmpty(result))
                return NotFound();

            return Ok(result);
        }

        [Route("api/movies/user/{userId}")]
        [HttpGet]
        public IHttpActionResult GetTopMoviesByUser(int userId)
        {
            IEnumerable<MovieViewModel> result = MovieViewService.GetTopMoviesByUserScore(userId);

            if (IsNullOrEmpty(result))
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult UserReview(UserMovieReviewViewModel movieReview)
        {
            bool isModelValid = MovieValidationService.IsValid(movieReview);

            if (!isModelValid)
            {
                return BadRequest("Invalid user movie review details");
            }

            bool isValidUserMovie = MovieValidationService.IsValidUserMovie(movieReview);

            if (!isModelValid)
            {
                return NotFound();
            }

            MovieViewService.SubmitUserMovieReview(movieReview);
            
            return Ok();
        }

        // TODO : Move to class as extension method over IEnumerable 
        private Func<IEnumerable<MovieViewModel>, bool> IsNullOrEmpty = (list)
            => list == null || !list.Any();
    }
}
