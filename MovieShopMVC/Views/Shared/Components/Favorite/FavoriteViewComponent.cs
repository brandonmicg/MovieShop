using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Services;

namespace MovieShopMVC.Views.Shared.Components.Favorite
{
    public class FavoriteViewComponent : ViewComponent
    {
        private readonly ICurrentLoggedInUser _currentLoggedInUser;
        private readonly IUserService _userService;


        public FavoriteViewComponent(ICurrentLoggedInUser currentLoggedInUser, IUserService userService)
        {
            _currentLoggedInUser = currentLoggedInUser;
            _userService = userService;
        }


        public async Task<IViewComponentResult> InvokeAsync(int movieId)
        {

            var model = new FavoriteModel { 
                MovieId = movieId,
                Toggle = this.HttpContext.User.Identity.IsAuthenticated,
                IsFavorite = this.HttpContext.User.Identity.IsAuthenticated ? await _userService.FavoriteExists(_currentLoggedInUser.UserId, movieId) : false
            };

            return View(model);
        }
    }
}
