using JwtRefreshTokenDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtRefreshTokenDemo.Controllers
{
    [ApiController]
    [Route("api/check")]
    public class CheckApiController : ControllerBase
    {
        public readonly GetDataService _getDataService;
        public CheckApiController(GetDataService getDataService)
        {
            _getDataService = getDataService;
        }   
        //[Authorize]
        [HttpGet("secure")]
        public IActionResult SecureApi()
        {
            return Ok(new
            {
                message = "Access granted",
                user = User.Identity?.Name
            });
        }
        [HttpGet("long-process")]
        public async Task<IActionResult> LongProcess()
        {
            try
            {
                var result = await _getDataService.ProcessAsync(HttpContext.RequestAborted);
                return Ok(result);
            }
            catch (OperationCanceledException)
            {
                // Log cancellation (optional)
                Console.WriteLine("Request cancelled by client");
                return StatusCode(499, "Request cancelled"); // Client Closed Request
            }
        }
    }
}
