using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);

            return View(movie);
        }

        public async Task<IActionResult> Genres(int id, int pageSize = 30, int pageNumber = 1)
        {
            var movies = await _movieService.GetMoviesByGenre(id, pageSize, pageNumber);

            return View("PagedMovies", movies);
        }
    }
}
