using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IReportRepository _reportService;

        public AdminService(IMovieRepository movieRepository, IReportRepository reportRepository)
        {
            _movieRepository = movieRepository;
            _reportService = reportRepository;
        }

        public async Task<bool> CreateMovie(MovieCreateRequest createRequest)
        {
            var newMovie = new Movie
            {
                Title = createRequest.Title,
                Overview = createRequest.Overview,
                Tagline = createRequest.Tagline,
                Revenue = createRequest.Revenue,
                Budget = createRequest.Budget,
                ImdbUrl = createRequest.ImdbUrl,
                TmdbUrl = createRequest.TmdbUrl,
                PosterUrl = createRequest.PosterUrl,
                BackdropUrl = createRequest.BackdropUrl,
                OriginalLanguage = createRequest.OriginalLanguage,
                ReleaseDate = createRequest.ReleaseDate,
                RunTime = createRequest.RunTime,
                Price = createRequest.Price,
                GenresOfMovies = new List<MovieGenre>(),
                Trailers = new List<Trailer>(),
                CastOfMovies = new List<MovieCast>(),
                CrewOfMovies = new List<MovieCrew>(),
                ReviewOfMovies = new List<Review>(),
                FavoritesOfMovies = new List<Favorite>(),
                PurchasesOfMovies = new List<Purchase>(),
            };

            foreach (var genre in createRequest.Genres)
            {
                newMovie.GenresOfMovies.Add(new MovieGenre
                {
                    GenreId = genre.Id
                });
            }
            

            var movie = await _movieRepository.Add(newMovie);

            if (movie.Id > 0)
                return true;

            return false;
        }

        public async Task<IEnumerable<MoviesReportModel>> GetTopPurchases([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, [FromQuery] int pageSize = 30, [FromQuery] int pageIndex = 1)
        {
            var movies = await _reportService.GetTopPurchasedMovies(fromDate, toDate, pageSize, pageIndex);

            if (movies == null || !movies.Any())
                return null;

            return movies;
        }

        public async Task<bool> UpdateMovie(MovieCreateRequest createRequest)
        {
            var newMovie = new Movie
            {
                Id = createRequest.Id,
                Title = createRequest.Title,
                Overview = createRequest.Overview,
                Tagline = createRequest.Tagline,
                Revenue = createRequest.Revenue,
                Budget = createRequest.Budget,
                ImdbUrl = createRequest.ImdbUrl,
                TmdbUrl = createRequest.TmdbUrl,
                PosterUrl = createRequest.PosterUrl,
                BackdropUrl = createRequest.BackdropUrl,
                OriginalLanguage = createRequest.OriginalLanguage,
                ReleaseDate = createRequest.ReleaseDate,
                RunTime = createRequest.RunTime,
                Price = createRequest.Price,
                GenresOfMovies = new List<MovieGenre>(),
            };

            
            foreach (var genre in createRequest.Genres)
            {
                newMovie.GenresOfMovies.Add(new MovieGenre
                {
                    MovieId = newMovie.Id,
                    GenreId = genre.Id
                });
            }


            var movie = await _movieRepository.Update(newMovie);

            if (movie.Id > 0)
                return true;

            return false;
        }
    }
}
