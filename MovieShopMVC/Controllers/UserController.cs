using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Services;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ICurrentLoggedInUser _currentLoggedInUser;
        private readonly IUserService _userService;

        public UserController(ICurrentLoggedInUser currentLoggedInUser, IUserService userService)
        {
            _currentLoggedInUser = currentLoggedInUser;
            _userService = userService;
        }

        //Purchases, that will give list of movies user purchased and should return a View that will show MovieCards and should use MovieCard partial view.
        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            //var cookie = this.HttpContext.Request.Cookies["MovieShopAuthCookie"];

            var userId = _currentLoggedInUser.UserId;

            //get all movies purchased by current user
            var movies = await _userService.GetAllPurchasesForUserId(userId);

            return View(movies);
        }

        //Favorites, that will give list of movies user Favorited and should return a View that will show MovieCards and should use MovieCard partial view.
        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var userId = _currentLoggedInUser.UserId;

            var movies = await _userService.GetAllFavoritesForUser(userId);

            return View(movies);
        }

        [HttpPost]
        public async Task<IActionResult> ReviewAction(ReviewRequestModel model, string submitButton)
        {
            switch (submitButton)
            {
                case "Submit":
                    return await AddReview(model);
                case "Delete":
                    return await DeleteReview(model);
            }

            return LocalRedirect("~/");
        }

        //Review for user to add a new Review, when user clicks on Review button in Movie Details Page and Review Confirmation Popup
        //[HttpPost]
        public async Task<IActionResult> AddReview(ReviewRequestModel model)
        {
            
            //get review for user and movie
            var userId = _currentLoggedInUser.UserId;
            var review = await _userService.GetReview(userId, model.MovieId);

            //if review exists, add review
            if(review == null)
            {
                var res = await _userService.AddMovieReview(model);
            }
            else
            {
                //else, update review
                var res = await _userService.UpdateMovieReview(model);
            }
            

            return RedirectToAction("Details", "Movies", new { id = model.MovieId });
        }

        //[HttpPost]
        public async Task<IActionResult> DeleteReview(ReviewRequestModel model)
        {
            
            //get review for user and movie
            var userId = _currentLoggedInUser.UserId;
            var review = await _userService.GetReview(userId, model.MovieId);

            //if review exists, delete review
            if(review != null)
            {
                var res = await _userService.DeleteMovieReview(userId, model.MovieId);
            }
            

            return RedirectToAction("Details", "Movies", new { id = model.MovieId });
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite(int movieId)
        {
            //check if already favorited
            var userId = _currentLoggedInUser.UserId;
            var favorited = await _userService.FavoriteExists(userId, movieId);

            var model = new FavoriteRequestModel
            {
                MovieId = movieId,
                UserId = userId
            };

            //update favorite based on current favorited status
            if (favorited)
            {

                var fav = await _userService.GetFavoriteById(userId, movieId);
                model.Id = fav.Id;
                var res = await _userService.RemoveFavorite(model);
            }
            else
            {
                var res = await _userService.AddFavorite(model);
            }

            
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        //Buy for user to buy a movie, when user click on Purchase button in Movie Details Page Purchase Confirmation Popup
        [HttpPost]
        public async Task<IActionResult> BuyMovie(int movieId)
        {
            //check if movie already purchased
            var userId = _currentLoggedInUser.UserId;
            var purchased = await _userService.IsMoviePurchased(userId, movieId);

            //add to purchase db/user
            if (!purchased)
            {
                var purchaseRequest = new PurchaseRequestModel
                {
                    MovieId = movieId,
                    PurchaseDate = DateTime.Now,                 
                };

                var purchase = await _userService.PurchaseMovie(purchaseRequest, userId);
            }
         
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            return View();
        }
    }
}
