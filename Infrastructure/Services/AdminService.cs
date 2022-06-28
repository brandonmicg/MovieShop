using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        public Task<IEnumerable<PurchaseRequestModel>> GetTopPurchases([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, [FromQuery] int pageSize = 30, [FromQuery] int pageIndex = 1)
        {
            throw new NotImplementedException();
        }
    }
}
