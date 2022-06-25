using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<IEnumerable<Movie>> Get30HighestGrossingMovies();
        Task<IEnumerable<Movie>> Get30HighestRatedMovies();
        Task<PagedResultSetModel<Movie>> GetMoviesByGenre(int genreId, int pageSize = 30, int pageNumber = 1);
        Task<decimal> GetAverageRatingForMovie(int movieId);
    }
}
