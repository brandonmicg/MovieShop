using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShopAPI.Services;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentLoggedInUser _currentLoggedInUser;

        public UserController(ICurrentLoggedInUser currentLoggedInUser, IUserService userService)
        {
            _currentLoggedInUser = currentLoggedInUser;
            _userService = userService;
        }

        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = _currentLoggedInUser.UserId;
            var user = await _userService.GetUserDetailsById(userId);

            if(user == null)
                return NotFound(new { errorMessage = "User not found" });

            return Ok(user);
        }

        [HttpGet]
        [Route("purchase-details/{movieId:int}")]
        public async Task<IActionResult> GetUserPurchaseDetails(int movieId)
        {
            var userId = _currentLoggedInUser.UserId;
            var purchase = await _userService.GetMoviePurchaseById(userId, movieId);

            if (purchase == null)
                return NotFound(new { errorMessage = "Movie purchase not found" });

            return Ok(purchase);
        }

        [HttpGet]
        [Route("check-movie-purchased/{movieId:int}")]
        public async Task<IActionResult> CheckMoviePurchased(int movieId)
        {
            var userId = _currentLoggedInUser.UserId;
            var purchased = await _userService.IsMoviePurchased(userId, movieId);

            return Ok(purchased);
        }

        [HttpPost]
        [Route("purchase-movie")]
        public async Task<IActionResult> PurchaseMovie([FromBody] int movieId)
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

                return Ok(purchase);
            }

            return Ok(new {message = "User already purchased this movie"});
        }

        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> GetUserPurchasedMovies()
        {
            var userId = _currentLoggedInUser.UserId;
            var movies = await _userService.GetAllPurchasesForUserId(userId);

            if(movies == null || !movies.Any())
                return NotFound(new {errorMessage = "Movies not found"});

            return Ok(movies);
        }

        [HttpPost]
        [Route("favorite")]
        public async Task<IActionResult> AddFavorite([FromBody] int movieId)
        {
            //check if already favorited
            var userId = _currentLoggedInUser.UserId;
            var favorited = await _userService.FavoriteExists(userId, movieId);

            var model = new FavoriteRequestModel
            {
                MovieId = movieId,
                UserId = userId
            };

            if (!favorited)
            {
                var res = await _userService.AddFavorite(model);
                return Ok(res);
            }

            return Ok(false);
        }

        [HttpDelete]
        [Route("un-favorite")]
        public async Task<IActionResult> RemoveFavorite([FromBody] int movieId)
        {
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

                return Ok(res);
            }

            return Ok(false);
        }

        [HttpGet]
        [Route("check-movie-favorite/{movieId:int}")]
        public async Task<IActionResult> CheckMovieIsFavorited(int movieId)
        {
            var userId = _currentLoggedInUser.UserId;
            var favorited = await _userService.FavoriteExists(userId, movieId);

            return Ok(favorited);
        }

        [HttpGet]
        [Route("favorites")]
        public async Task<IActionResult> GetUserFavoriteMovies()
        {
            var userId = _currentLoggedInUser.UserId;
            var movies = await _userService.GetAllFavoritesForUser(userId);

            if (movies == null || !movies.Any())
                return NotFound(new { errorMessage = "Movies not found" });

            return Ok(movies);
        }

        [HttpPost]
        [Route("add-review")]
        public async Task<IActionResult> AddReview([FromBody] ReviewRequestModel model)
        {
            //get review for user and movie
            var userId = _currentLoggedInUser.UserId;
            var review = await _userService.GetReview(userId, model.MovieId);

            //if review doesn't exists, add review
            if (review.UserId == 0)
            {
                var res = await _userService.AddMovieReview(model);
                return Ok(res);
            }

            return NotFound(new {message = "Cannot add review"});
        }

        [HttpPut]
        [Route("edit-review")]
        public async Task<IActionResult> EditReview([FromBody] ReviewRequestModel model)
        {
            //get review for user and movie
            var userId = _currentLoggedInUser.UserId;
            var review = await _userService.GetReview(userId, model.MovieId);

            //if review exists, update review
            if(review.UserId > 0)
            {
                var res = await _userService.UpdateMovieReview(model);
                return Ok(res);
            }

            return NotFound(new { message = "Cannot update review" });
        }

        [HttpDelete]
        [Route("delete-review")]
        public async Task<IActionResult> DeleteReview([FromBody] ReviewRequestModel model)
        {
            //get review for user and movie
            var userId = _currentLoggedInUser.UserId;
            var review = await _userService.GetReview(userId, model.MovieId);

            //if review exists, delete review
            if (review.UserId > 0)
            {
                var res = await _userService.DeleteMovieReview(userId, model.MovieId);
                return Ok(res);
            }

            return NotFound(new { message = "Cannot delete review" });
        }

        [HttpGet]
        [Route("movie-reviews")]
        public async Task<IActionResult> GetUserReviews()
        {
            var userId = _currentLoggedInUser.UserId;

            var reviews = await _userService.GetAllReviewsByUser(userId);

            if (reviews == null || !reviews.Any())
                return NotFound(new { errorMessage = "No user reviews found" });

            return Ok(reviews);
        }
    }
}
