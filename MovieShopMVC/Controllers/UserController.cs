using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Services;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ICurrentLoggedInUser _currentLoggedInUser;

        public UserController(ICurrentLoggedInUser currentLoggedInUser)
        {
            _currentLoggedInUser = currentLoggedInUser;
        }

        //Purchases, that will give list of movies user purchased and should return a View that will show MovieCards and should use MovieCard partial view.
        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            //var cookie = this.HttpContext.Request.Cookies["MovieShopAuthCookie"];

            var userId = _currentLoggedInUser.UserId;

            return View();
        }

        //Favorites, that will give list of movies user Favorited and should return a View that will show MovieCards and should use MovieCard partial view.
        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            return View();
        }

        //Review for user to add a new Review, when user clicks on Review button in Movie Details Page and Review Confirmation Popup
        [HttpPost]
        public async Task<IActionResult> AddReview()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite()
        {
            return View();
        }

        //Buy for user to buy a movie, when user click on Purchase button in Movie Details Page Purchase Confirmation Popup
        [HttpPost]
        public async Task<IActionResult> BuyMovie()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            return View();
        }
    }
}
