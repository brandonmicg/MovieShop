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

        //public async Task<IActionResult> PurchaseMovie()

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

        //public async Task<IActionResult> AddFavorite()

        //public async Task<IActionResult> RemoveFavorite()

        //public async Task<IActionResult> CheckMovieIsFavorited()

        //public async Task<IActionResult> GetUserFavoriteMovies()

        //public async Task<IActionResult> AddReview()

        //public async Task<IActionResult> EditReview()

        //public async Task<IActionResult> DeleteReview()

        //public async Task<IActionResult> GetUserReviews()
    }
}
