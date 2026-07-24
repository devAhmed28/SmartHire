using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Users;

namespace SmartHire.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<Result<UserProfileResponse>>
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
