namespace SmartHire.Application.DTOs.Users
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public bool IsActive { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
