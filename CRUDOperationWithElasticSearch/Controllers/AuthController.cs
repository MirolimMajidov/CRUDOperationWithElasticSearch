using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyUser.Models;
using MyUser.Models.Helpers;
using MyUser.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyUser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        protected readonly IEntityRepository<User> _repository;
        public AuthController(IEntityRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            User user = (await _repository.GetAllAsync()).SingleOrDefault(x => x.Username == username && x.Password == password);
            if (user is null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(accessToken);
        }
    }
}