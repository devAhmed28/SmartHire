using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.Features.Uploads.Commands.DeleteFile;

namespace SmartHire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UploadsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile([FromQuery] string publicId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(publicId))
            {
                return BadRequest("Public ID is required");
            }

            var command = new DeleteFileCommand
            {
                PublicId = publicId
            };

            var result = await _mediator.Send(command, cancellationToken);
            return ToActionResult(result);
        }
    }
}
