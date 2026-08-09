using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Users;

namespace SmartHire.Application.Features.Users.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserProfileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCurrentUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserProfileResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            var response = new UserProfileResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                IsEmailConfirmed = user.IsEmailConfirmed,
                IsActive = user.IsActive,
                ProfileImageUrl = user.ProfileImageUrl,
            };

            return Result.Success(response);
        }
    }
}
