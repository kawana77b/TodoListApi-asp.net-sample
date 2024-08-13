using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TodoApi.Models.Auth;

namespace TodoApi.Service.Identity
{
    /// <summary>
    /// Manage AppUser
    /// </summary>
    public class AppUserService(UserManager<AppUser> userManager)
    {
        public const string AdminRoleName = "Admin";

        protected readonly UserManager<AppUser> _userManager = userManager;

        /// <summary>
        /// Obtain whether AppUser and principal are identical
        /// </summary>
        /// <param name="user"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        public bool IsEqual(AppUser user, ClaimsPrincipal principal)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = principal.FindFirstValue(ClaimTypes.Name);
            return user.Id == userId && user.Email == email;
        }

        /// <summary>
        /// Retrieve the app user
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public async Task<AppUser?> GetUserAsync(ClaimsPrincipal principal)
        {
            var email = principal?.Identity?.Name;
            if (email is null)
            {
                return null;
            }
            return await GetUserAsync(email);
        }

        /// <summary>
        /// Retrieve the app user
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public async Task<string> GetUserIdAsync(ClaimsPrincipal principal)
        {
            var user = await GetUserAsync(principal);
            return user?.Id ?? string.Empty;
        }

        /// <summary>
        /// Retrieve the user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<AppUser?> GetUserAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Get whether the user is an Admin or not
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> IsAdminUserAsync(AppUser user)
        {
            return await HasRoleAsync(user, AdminRoleName);
        }

        /// <summary>
        /// Retrieve user claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }

        /// <summary>
        /// Get whether the user has the specified role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> HasRoleAsync(AppUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        /// <summary>
        /// Retrive user roles
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string[]> GetRolesAsync(AppUser user)
        {
            return [.. await _userManager.GetRolesAsync(user)];
        }
    }
}