using ApplicationCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [Route("top-grossing")]
        public async Task<IActionResult> GetTopGrossingMovies()
        {
            var movies = await _movieService.GetTopGrossingMovies();

            if(movies == null || !movies.Any())
            {
                return NotFound(new {errorMessage = "No Movies Found"});
            }

            return Ok(movies);
        }

        [HttpGet]
        [Route("top-rated")]
        public async Task<IActionResult> GetTopRatedMovies()
        {
            var movies = await _movieService.GetTopRatedMovies();

            if (movies == null || !movies.Any())
            {
                return NotFound(new { errorMessage = "No Movies Found" });
            }

            return Ok(movies);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);

            if (movie == null || !(movie.Id > 0))
                return NotFound(new { errorMessage = $"Movie Not Found For {id}" });

            return Ok(movie);
        }

        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetMovies(int pageSize = 30, int pageIndex = 1)
        {
            var movies = await _movieService.GetMovies(pageSize, pageIndex);

            if (movies == null || movies.PagedData == null || !movies.PagedData.Any())
            {
                return NotFound(new { errorMessage = "No Movies Found" });
            }

            return Ok(movies);
        }


        [HttpGet]
        [Route("genre/{genreId:int}")]
        public async Task<IActionResult> GetMoviesOfGenre(int genreId, int pageSize = 30, int pageIndex = 1)
        {
            var movies = await _movieService.GetMoviesByGenre(genreId, pageSize, pageIndex);

            if (movies == null || movies.PagedData == null || !movies.PagedData.Any())
            {
                return NotFound(new { errorMessage = "No Movies Found" });
            }

            return Ok(movies);
        }

        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<IActionResult> GetMovieReviews(int id, int pageSize = 30, int pageIndex = 1)
        {
            return Ok();
        }
    }
}
