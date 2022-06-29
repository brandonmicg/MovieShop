using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IMovieService
    {
        Task<List<MovieCardModel>> GetTopGrossingMovies();
        Task<List<MovieCardModel>> GetTopRatedMovies();
        Task<MovieDetailsModel> GetMovieDetails(int id, int userId = -1);
        Task<PagedResultSetModel<MovieCardModel>> GetMoviesByGenre(int genreId, int pageSize = 30, int pageNumber = 1, int paginationRange = 5);
        Task<PagedResultSetModel<MovieCardModel>> GetMovies(int pageSize = 30, int pageNumber = 1, int paginationRange = 5);
        Task<PagedResultSetModel<ReviewRequestModel>> GetReviewsForMovie(int id, int pageSize = 30, int pageNumber = 1, int paginationRange = 5);
    }
}
