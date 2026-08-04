using SmartHire.Domain.Enums;

namespace SmartHire.Application.DTOs.Applications
{
    public class UpdateApplicationStatusRequest
    {
        public ApplicationStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}
