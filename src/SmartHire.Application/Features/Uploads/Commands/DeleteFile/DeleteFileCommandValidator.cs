using FluentValidation;

namespace SmartHire.Application.Features.Uploads.Commands.DeleteFile
{
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(x => x.PublicId)
                .NotEmpty().WithMessage("Public ID is required");
        }
    }
}
