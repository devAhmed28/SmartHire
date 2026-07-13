using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.Common.Models;

namespace SmartHire.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Error?.Code switch
        {
            "NotFound" => NotFound(result),
            "Conflict" => Conflict(result),
            "Validation.Error" => BadRequest(result),
            "Unauthorized" => Unauthorized(result),
            "Forbidden" => Forbid(),
            "BadRequest" => BadRequest(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result)
        };
    }

    protected IActionResult ToActionResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok(new { message = "Operation completed successfully" });
        }

        return result.Error?.Code switch
        {
            "NotFound" => NotFound(result),
            "Conflict" => Conflict(result),
            "Validation.Error" => BadRequest(result),
            "Unauthorized" => Unauthorized(result),
            "Forbidden" => Forbid(),
            "BadRequest" => BadRequest(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result)
        };
    }
}