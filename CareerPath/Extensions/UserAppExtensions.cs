using System.Security.Claims;

namespace CareerPath.Extensions
{
    public static class UserAppExtensions
    {
        public static string UserId(this ClaimsPrincipal claims)
        => claims.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    }
}


