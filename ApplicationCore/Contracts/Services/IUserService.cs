﻿using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IUserService
    {
        Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId);
        Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId);
        Task<bool> IsMoviePurchased(int userId, int movieId);
        Task<IEnumerable<PurchaseRequestModel>> GetAllPurchasesForUserId(int id);
        //Task<> AddFavorite(FavoriteRequestModel favoriteRequest);
        //Task<> RemoveFavorite(FavoriteRequestModel favoriteRequest);
        Task<bool> FavoriteExists(int id, int movieId);
        //Task<> GetAllFavoritesForUser(int id);
        //Task<> AddMovieReview(ReviewRequestModel reviewRequest);
        //Task<> UpdateMovieReview(ReviewRequestModel reviewRequest);
        //Task<> DeleteMovieReview(int userId, int movieId);
        //Task<> GetAllReviewsByUser(int id);
    }
}
