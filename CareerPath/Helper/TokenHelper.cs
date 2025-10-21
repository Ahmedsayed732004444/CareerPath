using CareerPath.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CareerPath.Helper
{
    public class TokenHelper(IConfiguration _configuration) : ITokenHelper
    {

        public (string token, int expiresIn) GenerateToken(UserApp userApp, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresInMinutes = _configuration.GetValue<int>("JWT:expires");

            var tokenClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userApp.Id),
                new Claim(ClaimTypes.Email, userApp.Email!),
                new Claim(ClaimTypes.GivenName, userApp.UserName!),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                claims: tokenClaims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresInMinutes);
        }

        public string? ValidateToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
