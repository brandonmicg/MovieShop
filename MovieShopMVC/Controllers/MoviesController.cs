using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Services;

namespace MovieShopMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ICurrentLoggedInUser _currentLoggedInUser;
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService, ICurrentLoggedInUser currentLoggedInUser)
        {
            _movieService = movieService;
            _currentLoggedInUser = currentLoggedInUser;
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = _currentLoggedInUser.IsAuthenticated ? _currentLoggedInUser.UserId : -1;

            var movie = await _movieService.GetMovieDetails(id, userId);                 

            return View(movie);
        }

        public async Task<IActionResult> Genres(int id, int pageSize = 30, int pageNumber = 1)
        {
            var movies = await _movieService.GetMoviesByGenre(id, pageSize, pageNumber);

            return View("PagedMovies", movies);
        }
    }
}
