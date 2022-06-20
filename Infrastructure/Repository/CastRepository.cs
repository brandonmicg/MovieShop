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
    public class CastRepository : Repository<Cast>, ICastRepository
    {
        public CastRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public override Cast GetById(int id)
        {
            var castDetails = _dbContext.Casts
                .Include(c => c.MoviesOfCast).ThenInclude(c => c.Movie)
                .FirstOrDefault(c => c.Id == id);

            return castDetails;
        }
    }
}
