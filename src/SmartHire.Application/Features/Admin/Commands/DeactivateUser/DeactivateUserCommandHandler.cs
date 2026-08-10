using MediatR;
using SmartHire.Application.Common.Interfaces;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Admin;

namespace SmartHire.Application.Features.Admin.Commands.DeactivateUser
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Result<UserAdminResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserAdminResponse>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request .UserId, cancellationToken);

            if (user == null)
            {
                return Error.NotFound("User");
            }

            if (!user.IsActive)
            {
                return Error.Conflict("User is already deactivated");
            }

            user.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new UserAdminResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                IsEmailConfirmed = user.IsEmailConfirmed,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };

            return Result.Success(response);
        }
    }
}
