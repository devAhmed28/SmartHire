using FluentValidation;

namespace SmartHire.Application.Features.Interviews.Commands.ScheduleInterview
{
    public class ScheduleInterviewCommandValidator : AbstractValidator<ScheduleInterviewCommand>
    {
        public ScheduleInterviewCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("Company ID is required");

            RuleFor(x => x.ApplicationId)
                .NotEmpty().WithMessage("Application ID is required");

            RuleFor(x => x.ScheduledDate)
                .NotEmpty().WithMessage("Scheduled date is required")
                .GreaterThan(DateTime.UtcNow).WithMessage("Scheduled date must be in the future");

            RuleFor(x => x.InterviewType)
                .IsInEnum().WithMessage("Invalid interview type");

            RuleFor(x => x.MeetingLink)
                .MaximumLength(500).WithMessage("Meeting link must not exceed 500 characters")
                .Matches(@"^https?:\/\/[^\s]+$").WithMessage("Meeting link must be a valid URL")
                .When(x => !string.IsNullOrEmpty(x.MeetingLink));

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters");
        }
    }
}
