namespace LMS_API.Services.Contract
{
    public interface ITokenService
    {
        string GenerateToken(int userId, string email, string role);
        DateTime GetTokenExpiryUtc();
    }
}