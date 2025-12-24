using Microsoft.AspNetCore.Mvc;

namespace JwtRefreshTokenDemo.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            // Static credentials
            var username = "admin";
            var password = "password";

            if (username != "admin" || password != "password")
                return Unauthorized();

            var accessToken = _tokenService.GenerateAccessToken(username);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Ok(new
            {
                accessToken,
                refreshToken,
                expiresIn = 120
            });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] string refreshToken)
        {
            if (!_tokenService.ValidateRefreshToken(refreshToken))
                return Unauthorized("Invalid refresh token");

            var newAccessToken =
                _tokenService.GenerateAccessToken("admin");

            // rotate refresh token
            var newRefreshToken =
                _tokenService.RotateRefreshToken(refreshToken);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] string refreshToken)
        {
            _tokenService.RevokeRefreshToken(refreshToken);
            return Ok("Logged out successfully");
        }

    }
}
