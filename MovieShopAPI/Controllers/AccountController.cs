using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            var user = await _accountService.RegisterUser(model);

            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var user = await _accountService.ValidateUser(model.Email, model.Password);

            if (user != null) {
                var jwtToken = CreateJwtToken(user);
                return Ok( new { token = jwtToken});
            }

            throw new UnauthorizedAccessException("Please check email and password");

            return Unauthorized(new { errorMessage = "Please check email and password" });

        }

        private string CreateJwtToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim("Country", "USA"),
                    new Claim("Language", "english")
            };

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["secretKey"]));

            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenExpiration = DateTime.UtcNow.AddHours(2);

            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = tokenExpiration,
                SigningCredentials = credentials,
                Issuer = "MovieShop, Inc",
                Audience = "MovieShop Clients"
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var encodedJwt = tokenHandler.CreateToken(tokenDetails);

            return tokenHandler.WriteToken(encodedJwt);
        }

        [HttpGet]
        [Route("check-email")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var exists = await _accountService.EmailExists(email);

            if (!exists)
                return NotFound();

            return Ok(exists);
        }
    }
}
