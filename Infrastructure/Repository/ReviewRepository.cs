using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public Task<int> GetAverageMovieRating(int movieId)
        {
            throw new NotImplementedException();
        }

        public async Task<Review> GetReview(int userId, int movieId)
        {
            var review = await _dbContext.Reviews
                .Where(x => x.UserId == userId && x.MovieId == movieId)
                .FirstOrDefaultAsync();

            return review;
        }
    }
}
