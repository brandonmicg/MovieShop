using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {

        Task<int> GetAverageMovieRating(int movieId);
        Task<Review> GetReview(int userId, int movieId);
        Task<IEnumerable<Review>> GetAllReviewsByUser(int id);
        Task<PagedResultSetModel<Review>> GetMovieReviews(int movieId, int pageSize = 30, int pageNumber = 1, int paginationRange = 5);
    }
}
