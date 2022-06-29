using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost]
        [Route("movie")]
        public async Task<IActionResult> CreateMovie([FromBody] MovieCreateRequest createRequest)
        {

            var created = await _adminService.CreateMovie(createRequest);

            if(created)
                return Ok();

            return StatusCode(500, "Could not create movie");
        }

        [HttpPut]
        [Route("movie")]
        public async Task<IActionResult> UpdateMovieDetails([FromBody] MovieCreateRequest createRequest)
        {

            var updated = await _adminService.UpdateMovie(createRequest);

            if (updated)
                return Ok();

            return StatusCode(500, "Could not update movie");
        }

        [HttpGet]
        [Route("movie")]
        public async Task<IActionResult> GetTopPurchasedMovies([FromQuery] DateTime? fromDate = null, [FromQuery] DateTime? toDate = null, [FromQuery] int pageSize = 30, [FromQuery] int pageIndex = 1)
        {
            var movies = await _adminService.GetTopPurchases(fromDate, toDate, pageSize, pageIndex);

            if (movies == null)
                return NotFound(new { errorMessage = "Movie purchase report not found" });

            return Ok(movies);
        }
    }
}
