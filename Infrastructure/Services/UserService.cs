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
        private readonly IUserRepository _userRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMovieRepository _movieRepository;

        public UserService(IPurchaseRepository purchaseRepository, IUserRepository userRepository, IMovieRepository movieRepository)
        {
            _userRepository = userRepository;
            _purchaseRepository = purchaseRepository;
            _movieRepository = movieRepository;
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

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            return await _purchaseRepository.CheckIfPurchaseExists(userId, purchaseRequest.MovieId);
        }

        public async Task<bool> IsMoviePurchased(int userId, int movieId)
        {
            return await _purchaseRepository.CheckIfPurchaseExists(userId, movieId);
        }

        public async Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId)
        {
            //check user exists
            var user = await _userRepository.GetById(userId);

            if (user == null)
                return false;

            var movie = await _movieRepository.GetById(purchaseRequest.MovieId);

            //create purchase object
            var newPurchase = new Purchase
            {
                MovieId = purchaseRequest.MovieId,
                UserId = userId,
                TotalPrice = (decimal)movie.Price,
                PurchaseDateTime = purchaseRequest.PurchaseDate
            };

            //save object to purchase repo
            var saved = await _purchaseRepository.Add(newPurchase);

            //returned if saved
            if (saved.Id > 1)
                return true;

            return false;
        }
    }
}
