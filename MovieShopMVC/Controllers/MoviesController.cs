using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Services;

namespace MovieShopMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ICurrentLoggedInUser _currentLoggedInUser;
        private readonly IMovieService _movieService;
        private readonly IUserService _userService;

        public MoviesController(IMovieService movieService, ICurrentLoggedInUser currentLoggedInUser, IUserService userService)
        {
            _movieService = movieService;
            _currentLoggedInUser = currentLoggedInUser;
            _userService = userService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = - 1;
            var reviewModel = new ReviewRequestModel { Rating = 0, ReviewText = "" };

            if (this.HttpContext.User.Identity.IsAuthenticated)
            {
                userId = _currentLoggedInUser.UserId;
                reviewModel = await _userService.GetReview(userId, id);

                if(reviewModel.UserId == 0)
                {
                    reviewModel.UserId = userId;
                    reviewModel.MovieId = id;
                }
                    
            }

            var movie = await _movieService.GetMovieDetails(id, userId);
            movie.Review = reviewModel;
            

            return View(movie);
        }

        public async Task<IActionResult> Genres(int id, int pageSize = 30, int pageNumber = 1)
        {
            var movies = await _movieService.GetMoviesByGenre(id, pageSize, pageNumber);

            return View("PagedMovies", movies);
        }
    }
}
