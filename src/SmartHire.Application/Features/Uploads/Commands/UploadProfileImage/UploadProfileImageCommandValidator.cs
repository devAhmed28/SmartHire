using FluentValidation;

namespace SmartHire.Application.Features.Uploads.Commands.UploadProfileImage
{
    public class UploadProfileImageCommandValidator : AbstractValidator<UploadProfileImageCommand>
    {
        public UploadProfileImageCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required");

            When(x => x.File != null, () =>
            {
                RuleFor(x => x.File.Length)
                    .GreaterThan(0).WithMessage("File is empty");

                RuleFor(x => x.File.Length)
                    .LessThanOrEqualTo(2 * 1024 * 1024)
                    .WithMessage("File size must not exceed 2MB");

                RuleFor(x => x.File.FileName)
                    .Must(HaveAllowedExtension)
                    .WithMessage("Only JPG, PNG, and GIF files are allowed");
            });
        }

        private bool HaveAllowedExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(fileName).ToLower();
            return allowedExtensions.Contains(extension);
        }
    }
}
