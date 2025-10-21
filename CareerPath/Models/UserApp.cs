using Microsoft.AspNetCore.Identity;

namespace CareerPath.Models
{
    public class UserApp : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
