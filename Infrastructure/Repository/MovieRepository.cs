using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Contracts;
using ApplicationCore.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Movie> GetHighestGrossingMovies()
        {
            var movies = _dbContext.Movies.OrderByDescending(x => x.Revenue).Take(30).ToList();
            return movies;
        }

        public IEnumerable<Movie> GetHighestRatedMovies()
        {
            throw new NotImplementedException();
        }
    }
}
