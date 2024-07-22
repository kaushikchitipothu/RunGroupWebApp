using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace RunGroupWebApp
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserID(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
