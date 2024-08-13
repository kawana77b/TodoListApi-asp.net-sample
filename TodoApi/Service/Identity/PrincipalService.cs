using System.Security.Claims;

namespace TodoApi.Service.Identity
{
    /// <summary>
    /// Manage user information through <c>ClaimsPrincipal</c>.
    /// </summary>
    public class PrincipalService
    {
        /// <summary>
        /// Get user's ID
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public string? GetUserId(ClaimsPrincipal principal)
        {
            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return id ?? string.Empty;
        }

        /// <summary>
        /// Get user's email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string? GetUserEmail(ClaimsPrincipal principal)
        {
            var email = principal?.Identity?.Name;
            return email;
        }

        /// <summary>
        /// Get whether the user is an Admin
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsAdminUser(ClaimsPrincipal principal)
        {
            return principal.IsInRole("Admin");
        }

        public bool HasClaim(ClaimsPrincipal principal, string claimType)
        {
            return principal.HasClaim(x => x.Type == claimType);
        }

        /// <summary>
        /// Get user's claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaims(ClaimsPrincipal principal)
        {
            return principal.Claims;
        }

        /// <summary>
        /// Get the value of the claim
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public string? GetClaimValue(ClaimsPrincipal principal, string claimType)
        {
            return principal.FindFirstValue(claimType);
        }

        /// <summary>
        /// Get whether the user has this role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool HasRole(ClaimsPrincipal principal, string role)
        {
            return principal.IsInRole(role);
        }

        /// <summary>
        /// Get the user's roles
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string[] GetRoles(ClaimsPrincipal principal)
        {
            return principal.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        }
    }
}