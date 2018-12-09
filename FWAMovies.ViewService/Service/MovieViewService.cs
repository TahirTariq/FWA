using AutoMapper;
using FWAMovies.BusinessService.Interface;
using FWAMovies.Model;
using FWAMovies.Model.Dto;
using FWAMovies.ViewService.ViewModel;
using System.Collections.Generic;

namespace FWAMovies.ViewService.Service
{
    public class MovieViewService : IMovieViewService
    {
        private IMovieBusinessService _movieBusinessService;

        public MovieViewService(IMovieBusinessService movieBusinessService)
        {
            _movieBusinessService = movieBusinessService;
        }

        public IEnumerable<MovieViewModel> GetTopMovies()
        {
            IEnumerable<Movie> model = _movieBusinessService.GetTopMovies();

            return Mapper.Map<IEnumerable<MovieViewModel>>(model);
        }

        public IEnumerable<MovieViewModel> GetMoviesBy(MovieFilterViewModel filter)
        {
            var movieFilter = AutoMapper.Mapper.Map<MovieFilter>(filter);

            IEnumerable<Movie> model = _movieBusinessService.GetTopMovies();

            return AutoMapper.Mapper.Map<IEnumerable<MovieViewModel>>(model);
        }

        public IEnumerable<MovieViewModel> GetTopMoviesByUserScore(int userId)
        {
            IEnumerable<Movie> model = _movieBusinessService.GetTopMoviesByUserScore(userId);

            return Mapper.Map<IEnumerable<MovieViewModel>>(model);
        }

        public UserMovieReviewViewModel SubmitUserMovieReview(UserMovieReviewViewModel userReviewView)
        {
            UserMovieReview userReview = Mapper.Map<UserMovieReview>(userReviewView);

            userReview = _movieBusinessService.SubmitUserMovieReview(userReview);

            return Mapper.Map<UserMovieReviewViewModel>(userReview);
        }
    }
}
