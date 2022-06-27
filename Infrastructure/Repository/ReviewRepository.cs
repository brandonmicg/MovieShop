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

        public async override Task<Review> Delete(Review entity)
        {
            var local = _dbContext.Set<Review>()
                .Local
                .FirstOrDefault(x => x.UserId == entity.UserId && x.MovieId == entity.MovieId);

            if (local != null)
            {
                _dbContext.Entry(local).State = EntityState.Detached;
            }

            _dbContext.Set<Review>().Remove(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async override Task<Review> Update(Review entity)
        {
            var local = _dbContext.Set<Review>()
                .Local
                .FirstOrDefault(x => x.UserId == entity.UserId && x.MovieId == entity.MovieId);

            if (local != null)
            {
                _dbContext.Entry(local).State = EntityState.Detached;
            }

            _dbContext.Set<Review>().Update(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }


    }
}
