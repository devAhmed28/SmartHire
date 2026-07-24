using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Users;

namespace SmartHire.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result<UserProfileResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProfileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserProfileResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            var existingUser = await _unitOfWork.Users.GetByPhoneNumberAsync(request.PhoneNumber, cancellationToken);

            if (existingUser != null && existingUser.Id != request.UserId)
            {
                return Error.Conflict($"Phone number '{request.PhoneNumber}' is already in use by another account");
            }

            user.UpdateProfile(
                request.FirstName,
                request.LastName,
                request.PhoneNumber
            );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new UserProfileResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                IsEmailConfirmed = user.IsEmailConfirmed,
                IsActive = user.IsActive
            };

            return Result.Success(response);
        }
    }
}
