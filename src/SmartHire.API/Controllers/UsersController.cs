using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.DTOs.Uploads;
using SmartHire.Application.DTOs.Users;
using SmartHire.Application.Features.Uploads.Commands.UploadProfileImage;
using SmartHire.Application.Features.Users.Commands.ChangePassword;
using SmartHire.Application.Features.Users.Commands.UpdateProfile;
using SmartHire.Application.Features.Users.Queries.GetCurrentUser;
using System.Security.Claims;

namespace SmartHire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var query = new GetCurrentUserQuery
            {
                UserId = userId.Value
            };

            var result = await _mediator.Send(query, cancellationToken);
            return ToActionResult(result);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new UpdateProfileCommand
            {
                UserId = userId.Value,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return BadRequest("New password and confirmation password do not match");
            }

            var command = new ChangePasswordCommand
            {
                UserId = userId.Value,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        [HttpPost("me/profile-image")]
        [RequestSizeLimit(2 * 1024 * 1024)]
        public async Task<IActionResult> UploadProfileImage([FromForm] UploadProfileImageRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var command = new UploadProfileImageCommand
            {
                UserId = userId.Value,
                File = request.File
            };

            var result = await _mediator.Send(command, cancellationToken);

            return ToActionResult(result);
        }

        private Guid? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                return null;

            return userId;
        }
    }
}
