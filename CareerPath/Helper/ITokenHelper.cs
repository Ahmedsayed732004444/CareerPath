using CareerPath.Models;

namespace CareerPath.Helper
{
    public interface ITokenHelper
    {
        (string token, int expiresIn) GenerateToken(UserApp userApp, string role);
        string? ValidateToken(string token);
    }
}
