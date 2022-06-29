using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repositories
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {

        Task<IEnumerable<Purchase>> GetPurchasesByUserId(int id);
        Task<Purchase> GetPurchaseByMovieUserId(int userId, int movieId);
        Task<bool> CheckIfPurchaseExists(int userId, int movieId);
    }
}
