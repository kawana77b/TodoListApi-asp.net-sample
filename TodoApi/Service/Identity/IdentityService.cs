using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TodoApi.Common;
using TodoApi.Common.Contracts;
using TodoApi.Models.Auth;

namespace TodoApi.Service.Identity
{
    /// <summary>
    /// Manage users, especially identities, on the database
    /// </summary>
    public class IdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : AppUserService(userManager)
    {
        private readonly SignInManager<AppUser> _signInManager = signInManager;

        /// <summary>
        /// ユーザの認証情報を取得する
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(AppUser user)
        {
            return await _signInManager.CreateUserPrincipalAsync(user);
        }

        /// <summary>
        /// register a user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        /// <remarks>To set up a role, the role must already be registered in the database</remarks>
        public async Task<IResultData<AppUser, IdentityResult>> RegisterAsync(string email, string password, string[]? roles = null)
        {
            var user = new AppUser
            {
                Email = email,
                UserName = email,
            };
            var userCreated = await _userManager.CreateAsync(user, password);
            if (!userCreated.Succeeded)
            {
                return ResultData<AppUser, IdentityResult>.Failure(userCreated);
            }
            if (roles is not null)
            {
                await AddRolesAsync(user, roles);
            }
            return ResultData<AppUser, IdentityResult>.Success(user);
        }

        /// <summary>
        /// Update the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdateAsync(AppUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Change the user's password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        /// <summary>
        /// Remove the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RemoveAsync(AppUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        /// <summary>
        /// Set roles for users. Existing roles are excluded from addition.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task AddRolesAsync(AppUser user, params string[] roles)
        {
            var currentRoles = await GetRolesAsync(user);
            await _userManager.AddToRolesAsync(user, roles.Except(currentRoles));
        }

        /// <summary>
        /// Delete a role from a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task RemoveRolesAsync(AppUser user, params string[] roles)
        {
            await _userManager.RemoveFromRolesAsync(user, roles);
        }
    }
}