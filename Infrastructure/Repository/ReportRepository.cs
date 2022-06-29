using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ReportRepository : Repository<Purchase>, IReportRepository
    {
        public ReportRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<MoviesReportModel>> GetTopPurchasedMovies(DateTime? fromDate = null, DateTime? toDate = null, int pageSize = 30, int pageIndex = 1)
        {
            var models = new List<MoviesReportModel>();

            var movies = await _dbContext.MovieReports
                .FromSqlInterpolated($"EXECUTE dbo.usp_GetTopPurchasedMovies {fromDate}, {toDate}, {pageSize}, {pageIndex}")               
                .ToListAsync();

            return movies;
        }
    }
}

