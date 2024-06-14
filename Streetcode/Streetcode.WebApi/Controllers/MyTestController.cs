using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Streetcode.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyController : BaseApiController
    {
        [HttpGet("test-error")]
        public async Task<IActionResult> TestError()
        {
            // Simulate an unauthorized access scenario
            var result = Result.Fail("401");
            return Unauthorized();
        }

        [HttpGet("test-not-found")]
        public async Task<IActionResult> TestNotFound()
        {
            // Simulate a resource not found scenario
            var result = Result.Fail("404");
            return BadRequest(result);
        }
    }

}
