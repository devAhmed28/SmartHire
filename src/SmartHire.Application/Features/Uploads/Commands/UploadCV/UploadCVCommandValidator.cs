using FluentValidation;

namespace SmartHire.Application.Features.Uploads.Commands.UploadCV
{
    public class UploadCVCommandValidator : AbstractValidator<UploadCVCommand>
    {
        public UploadCVCommandValidator()
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
                    .LessThanOrEqualTo(5 * 1024 * 1024)
                    .WithMessage("File size must not exceed 5MB");

                RuleFor(x => x.File.FileName)
                    .Must(HaveAllowedExtension)
                    .WithMessage("Only PDF, DOC, and DOCX files are allowed");
            });
        }

        private bool HaveAllowedExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var extension = Path.GetExtension(fileName).ToLower();
            return allowedExtensions.Contains(extension);
        }
    }
}