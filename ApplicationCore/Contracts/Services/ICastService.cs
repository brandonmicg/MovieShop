using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface ICastService
    {
        Task<CastDetailsModel> GetCastDetails(int id);
    }
}
