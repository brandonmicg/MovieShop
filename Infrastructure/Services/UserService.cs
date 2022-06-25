using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.Services;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        //private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public UserService(IMovieRepository movieRepository, IPurchaseRepository purchaseRepository)
        {
            //_userRepository = userRepository;
            _movieRepository = movieRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<IEnumerable<PurchaseRequestModel>> GetAllPurchasesForUserId(int id)
        {
            var purchaseRequests = new List<PurchaseRequestModel>();

            var purchases = await _purchaseRepository.GetPurchasesByUserId(id);

            foreach (var purchase in purchases)
            {
                purchaseRequests.Add(new PurchaseRequestModel
                {
                    MovieId = purchase.MovieId,
                    Title = purchase.Movie.Title,
                    PosterUrl = purchase.Movie.PosterUrl,
                    PurchaseDate = purchase.PurchaseDateTime,
                    Price = purchase.TotalPrice,
                    PurchaseNumber = purchase.PurchaseNumber
                });
             }

            return purchaseRequests;
        }
    }
}
