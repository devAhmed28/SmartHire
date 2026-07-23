using FluentValidation;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.AccountType)
            .NotNull().WithMessage("Account type is required")
            .IsInEnum().WithMessage("Account type must be Candidate (1) or Company (2)");

            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters")
                .Matches(@"^\+?[0-9\s\-\(\)]+$").WithMessage("Invalid phone number format");

            When(x => x.AccountType == AccountType.Candidate, () =>
            {
                RuleFor(x => x.Candidate)
                .NotNull().WithMessage("Candidate details are required for Candidate registration");

                RuleFor(x => x.Candidate!.FirstName)
                    .NotEmpty().WithMessage("First name is required")
                    .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

                RuleFor(x => x.Candidate!.LastName)
                    .NotEmpty().WithMessage("Last name is required")
                    .MaximumLength(50).WithMessage("Last name must not exceed 50 characters");
            });

            When(x => x.AccountType == AccountType.Company, () =>
            {
                RuleFor(x => x.Company)
                    .NotNull().WithMessage("Company details are required for Company registration");

                RuleFor(x => x.Company!.CompanyName)
                    .NotEmpty().WithMessage("Company name is required")
                    .MaximumLength(200).WithMessage("Company name must not exceed 200 characters");

                RuleFor(x => x.Company!.Description)
                    .MaximumLength(2000).WithMessage("Company description must not exceed 2000 characters");

                RuleFor(x => x.Company!.Industry)
                    .MaximumLength(100).WithMessage("Industry must not exceed 100 characters");

                RuleFor(x => x.Company!.CompanySize)
                    .IsInEnum().WithMessage("Invalid company size. Must be Startup, Small, Medium, Large, or Enterprise");
            });
        }
    }
}
