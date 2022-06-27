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
    public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Favorite> GetFavoriteById(int userId, int movieId)
        {
            var favorite = await _dbContext.Favorites.FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);
            return favorite;
        }

        public override async Task<Favorite> Delete(Favorite entity)
        {
            var local = _dbContext.Set<Favorite>()
                .Local
                .FirstOrDefault(x => x.Id == entity.Id);

            if (local != null)
            {
                _dbContext.Entry(local).State = EntityState.Detached;
            }

            _dbContext.Set<Favorite>().Remove(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

    }
}
