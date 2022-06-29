using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<MoviesReportModel>> GetTopPurchases([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, [FromQuery] int pageSize = 30, [FromQuery] int pageIndex = 1);

        Task<bool> CreateMovie(MovieCreateRequest createRequest);

        Task<bool> UpdateMovie(MovieCreateRequest createRequest);
    }
}
