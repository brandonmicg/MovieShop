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

        public CastDetailsModel GetCastDetails(int id)
        {
            var castDetails = _castRepository.GetById(id);

            //finish
            var cast = new CastDetailsModel();

            return cast;
        }
    }
}
