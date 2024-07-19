using Microsoft.AspNetCore.Identity;

namespace Chat.AuthService.Infrastructure
{
    /// <summary>
    /// Application role
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>
    {
    }
}