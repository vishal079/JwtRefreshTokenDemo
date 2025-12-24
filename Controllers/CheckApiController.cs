using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtRefreshTokenDemo.Controllers
{
    [ApiController]
    [Route("api/check")]
    public class CheckApiController : ControllerBase
    {
        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureApi()
        {
            return Ok(new
            {
                message = "Access granted",
                user = User.Identity?.Name
            });
        }
    }
}
