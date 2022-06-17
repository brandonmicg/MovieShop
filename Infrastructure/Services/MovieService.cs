using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Models;
using ApplicationCore.Services;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {

        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public MovieDetailsModel GetMovieDetails(int id)
        {
            var movie = new MovieDetailsModel { };

            return movie;
        }

        public List<MovieCardModel> GetTopGrossingMovies()
        {
            var movieCards = new List<MovieCardModel>();
            var movies = _movieRepository.Get30HighestGrossingMovies();

            foreach(var movie in movies)
            {
                movieCards.Add(new MovieCardModel() { Id = movie.Id, PosterUrl = movie.PosterUrl, Title = movie.Title});
            }

            return movieCards;
        }
        
    }
}
