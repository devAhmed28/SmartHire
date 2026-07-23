using MediatR;
using SmartHire.Application.Common.Models;
using SmartHire.Application.DTOs.Auth;
using SmartHire.Domain.Enums;

namespace SmartHire.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<Result<AuthResponse>>
    {
        public AccountType AccountType { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public CompanyInfo? Company { get; set; }
        public CandidateInfo? Candidate { get; set; }
    }
}
