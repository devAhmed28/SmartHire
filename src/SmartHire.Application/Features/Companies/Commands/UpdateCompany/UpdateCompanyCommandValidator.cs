using FluentValidation;

namespace SmartHire.Application.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(200).WithMessage("Company name must not exceed 200 characters");

            RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

            RuleFor(x => x.Industry)
                .MaximumLength(100).WithMessage("Industry must not exceed 100 characters");

            RuleFor(x => x.WebsiteUrl)
                .MaximumLength(500).WithMessage("Website URL must not exceed 500 characters")
                .Matches(@"^https?:\/\/[^\s]+$").WithMessage("Website URL must be a valid URL starting with http:// or https://")
                .When(x => !string.IsNullOrEmpty(x.WebsiteUrl));

            RuleFor(x => x.LinkedInUrl)
                .MaximumLength(500).WithMessage("LinkedIn URL must not exceed 500 characters")
                .Matches(@"^https?:\/\/[^\s]+$").WithMessage("LinkedIn URL must be a valid URL starting with http:// or https://")
                .When(x => !string.IsNullOrEmpty(x.LinkedInUrl));

            RuleFor(x => x.Address)
                .MaximumLength(300).WithMessage("Address must not exceed 300 characters");

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("City must not exceed 100 characters");

            RuleFor(x => x.Country)
                .MaximumLength(100).WithMessage("Country must not exceed 100 characters");

            RuleFor(x => x.FoundedYear)
                .InclusiveBetween(1900, DateTime.UtcNow.Year)
                .WithMessage($"Founded year must be between 1900 and {DateTime.UtcNow.Year}");

            RuleFor(x => x.CompanySize)
                .IsInEnum().WithMessage("Invalid company size");
        }
    }
}
