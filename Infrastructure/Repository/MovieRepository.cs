using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
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
            throw new NotImplementedException();
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
    }
}
