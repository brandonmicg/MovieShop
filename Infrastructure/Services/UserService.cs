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
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IReviewRepository _reviewRepository;

        public UserService(IPurchaseRepository purchaseRepository, IUserRepository userRepository, 
            IMovieRepository movieRepository, IFavoriteRepository favoriteRepository, IReviewRepository reviewRepository)
        {
            _userRepository = userRepository;
            _purchaseRepository = purchaseRepository;
            _movieRepository = movieRepository;
            _favoriteRepository = favoriteRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<bool> AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            var newFavorite = new Favorite
            {
                MovieId = favoriteRequest.MovieId,
                UserId = favoriteRequest.UserId
            };

            var saved = await _favoriteRepository.Add(newFavorite);

            if (saved.Id > 1)
                return true;

            return false;
        }

        public async Task<bool> AddMovieReview(ReviewRequestModel reviewRequest)
        {
            var newReview = new Review
            {
                MovieId = reviewRequest.MovieId,
                UserId = reviewRequest.UserId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText
            };

            var saved = await _reviewRepository.Add(newReview);

            if (saved.User != null)
                return true;

            return false;
        }

        public async Task<bool> DeleteMovieReview(int userId, int movieId)
        {
            var removed = await _reviewRepository.Delete(new Review { UserId = userId, MovieId = movieId});

            if (removed.UserId > 0)
                return false;

            return true;
        }

        public async Task<bool> FavoriteExists(int id, int movieId)
        {
            var favorite = await _favoriteRepository.GetFavoriteById(id, movieId);

            return favorite == null ? false : true;
        }

        public async Task<IEnumerable<MovieCardModel>> GetAllFavoritesForUser(int id)
        {
            List<MovieCardModel> movieModels = new List<MovieCardModel>();

            var movies = await _favoriteRepository.GetAllFavoriteMoviesByUserId(id);

            foreach (var movie in movies)
            {
                movieModels.Add(new MovieCardModel
                {
                    Id = movie.MovieId,
                    Title = movie.Movie.Title,
                    PosterUrl = movie.Movie.PosterUrl
                });
            }

            return movieModels;

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

        public async Task<Favorite> GetFavoriteById(int userId, int movieId)
        {
            var favorite = await _favoriteRepository.GetFavoriteById(userId, movieId);
            return favorite;
        }

        public async Task<ReviewRequestModel> GetReview(int userId, int movieId)
        {
            var review = await _reviewRepository.GetReview(userId, movieId);

            if (review == null)
                return new ReviewRequestModel { Rating = 0, ReviewText = ""};

            var model = new ReviewRequestModel
            {
                MovieId = review.MovieId,
                UserId = review.UserId,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
            };

            return model;
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
                PurchaseDateTime = purchaseRequest.PurchaseDate,
                PurchaseNumber = Guid.NewGuid()
            };

            //save object to purchase repo
            var saved = await _purchaseRepository.Add(newPurchase);

            //returned if saved
            if (saved.Id > 1)
                return true;

            return false;
        }

        public async Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            var remFavorite = new Favorite
            {
                Id = favoriteRequest.Id,
                MovieId = favoriteRequest.MovieId,
                UserId = favoriteRequest.UserId
            };

            var removed = await _favoriteRepository.Delete(remFavorite);

            
            if (removed.Id > 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            var updReview = new Review
            {
                MovieId = reviewRequest.MovieId,
                UserId = reviewRequest.UserId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText
            };

            var saved = await _reviewRepository.Update(updReview);

            if (saved.User != null)
                return true;

            return false;
        }
    }
}
