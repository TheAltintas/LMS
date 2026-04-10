namespace LMS_API.Models.DTO.Auth
{
    public class AuthResponseDTO
    {
        public required string Token { get; set; }
        public required string Role { get; set; }
        public required string Email { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}