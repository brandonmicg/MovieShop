using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts
{
    public interface IMovieRepository : IRepository<Movie>
    {
        IEnumerable<Movie> GetHighestGrossingMovies();
        IEnumerable<Movie> GetHighestRatedMovies();
    }
}
