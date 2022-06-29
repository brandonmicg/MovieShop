using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Movie>> Get30HighestGrossingMovies()
        {
            var movies = await _dbContext.Movies.OrderByDescending(x => x.Revenue).Take(30).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> Get30HighestRatedMovies()
        {
            //get id's of movies with best averaged rating
            var avgRatingIds = _dbContext.Reviews
                .GroupBy(g => g.MovieId, r => r.Rating)
                .Select(g => new
                {
                    MovieId = g.Key,
                    AvgRating = g.Average()
                })
                .OrderByDescending(x => x.AvgRating)
                .Take(30);

            //get all movies from db with ids from avgRating list
            /*
            var movies = await _dbContext.Movies
                .Where(x => avgRatingIds
                .Any(r => r.MovieId == x.Id)).ToListAsync();
            */

            
            var movies = await _dbContext.Movies
            .Join(
            avgRatingIds,
            movie => movie.Id,
            avg => avg.MovieId,
            (movie, avg) => new
            {
                Movie = movie,
                Rating = avg.AvgRating
            })
            .OrderByDescending(m => m.Rating)
            .Select(m => m.Movie)
            .ToListAsync();
            

            return movies;
        }

        public async Task<decimal> GetAverageRatingForMovie(int movieId)
        {
            var rating = await _dbContext.Reviews
                .Where(r => r.MovieId == movieId)
                .AverageAsync(x => x.Rating);

            return rating;
        }

        public async override Task<Movie> GetById(int id)
        {

            var movieDetails = await _dbContext.Movies
                .Include(m => m.GenresOfMovies).ThenInclude(m => m.Genre)
                .Include(m => m.Trailers)
                .Include(m => m.CastOfMovies).ThenInclude(m => m.Cast)             
                .FirstOrDefaultAsync(m => m.Id == id);

            return movieDetails;
        }

        public async Task<PagedResultSetModel<Movie>> GetMovies(int pageSize = 30, int pageNumber = 1, int paginationRange = 5)
        {
            var totalMovies = await _dbContext.Movies.CountAsync();

            var movies = await _dbContext.Movies
                .OrderByDescending(m => m.Revenue)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSetModel<Movie>(pageNumber, totalMovies, pageSize, movies, paginationRange);

            return pagedMovies;
        }

        public async Task<PagedResultSetModel<Movie>> GetMoviesByGenre(int genreId, int pageSize = 30, int pageNumber = 1, int paginationRange = 5)
        {
            var totalMoviesForGenre = await _dbContext.MovieGenres.Where(m => m.GenreId == genreId).CountAsync();

            var movies = await _dbContext.MovieGenres
                .Where(m => m.GenreId == genreId)
                .Include(m => m.Movie)
                .OrderByDescending(m => m.Movie.Revenue)
                .Select(m => new Movie { Id = m.MovieId, PosterUrl = m.Movie.PosterUrl, Title = m.Movie.Title })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSetModel<Movie>(pageNumber, totalMoviesForGenre, pageSize, movies, paginationRange);

            return pagedMovies;
        }

        public async override Task<Movie> Update(Movie entity)
        {
            var local = _dbContext.Set<Movie>()
                .Local
                .FirstOrDefault(x => x.Id == entity.Id);

            if (local != null)
            {
                _dbContext.Entry(local).State = EntityState.Detached;
            }

            _dbContext.Set<Movie>().Update(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
