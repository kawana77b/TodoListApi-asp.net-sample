using Microsoft.AspNetCore.Identity;
using TodoApi.Models.Attributes;

namespace TodoApi.Models.Auth
{
    /// <summary>
    /// Represents a user in the application.
    /// </summary>
    [Record]
    public class AppUser : IdentityUser
    {
    }
}