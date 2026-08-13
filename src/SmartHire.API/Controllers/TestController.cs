using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.Common.Models;

namespace SmartHire.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Just for testing purpose only...
    public class TestController : ApiControllerBase
    {
        /// <summary>
        /// GET: api/test
        /// Returns a simple message to confirm the API is running.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var result = Result.Success(new
            {
                message = "SmartHire API is running!",
                timestamp = DateTime.UtcNow
            });

            return ToActionResult(result);
        }

        /// <summary>
        /// GET: api/test/error
        /// Simulates an error to test the exception handling middleware.
        /// </summary>
        [HttpGet("error")]
        public IActionResult GetError()
        {
            throw new Exception("This is a test exception to verify exception handling!");
        }

        /// <summary>
        /// GET: api/test/notfound
        /// Simulates a "not found" error using Result pattern.
        /// </summary>
        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            var result = Result.Failure(Error.NotFound("Test Entity"));
            return ToActionResult(result);
        }

        /// <summary>
        /// GET: api/test/conflict
        /// Simulates a "conflict" error using Result pattern.
        /// </summary>
        [HttpGet("conflict")]
        public IActionResult GetConflict()
        {
            var result = Result.Failure(Error.Conflict("Test conflict occurred"));
            return ToActionResult(result);
        }
    }
}
