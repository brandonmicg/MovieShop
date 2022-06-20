using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Models;
using ApplicationCore.Services;

namespace Infrastructure.Services
{
    public class CastService : ICastService
    {
        private readonly ICastRepository _castRepository;

        public CastService(ICastRepository castRepository)
        {
            _castRepository = castRepository;
        }

        public async Task<CastDetailsModel> GetCastDetails(int id)
        {
            var castDetails = await _castRepository.GetById(id);

            
            var cast = new CastDetailsModel()
            {
                Id = castDetails.Id,
                Name = castDetails.Name,
                Gender = castDetails.Gender,
                ProfilePath = castDetails.ProfilePath
            };

            return cast;
        }
    }
}
