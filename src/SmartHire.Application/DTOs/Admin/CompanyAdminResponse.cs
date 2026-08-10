namespace SmartHire.Application.DTOs.Admin
{
    public class CompanyAdminResponse
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public int JobCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
