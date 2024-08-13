using Microsoft.AspNetCore.Identity;

namespace TodoApi.Service.Identity
{
    /// <summary>
    /// Manage roles, especially those tied on the database
    /// </summary>
    public class RoleService(RoleManager<IdentityRole> roleManager)
    {
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        /// <summary>
        /// Get whether the role exists or not
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        private async Task<bool> ExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        /// <summary>
        /// Gets the role. Return null if it does not exist.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<IdentityRole?> GetAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        /// <summary>
        /// Gets the all roles
        /// </summary>
        /// <returns></returns>
        public IdentityRole[] GetAllAsync()
        {
            return [.. _roleManager.Roles.Where(x => x != null).Select(x => x!)];
        }

        /// <summary>
        /// Add a role. If it already exists, do nothing.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task AddOrElseAsync(string roleName)
        {
            if (await ExistsAsync(roleName) == false)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        /// <summary>
        /// Update the role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task UpdateAsync(IdentityRole role)
        {
            await _roleManager.UpdateAsync(role);
        }

        /// <summary>
        /// Remove the role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task RemoveAsync(IdentityRole role)
        {
            await _roleManager.DeleteAsync(role);
        }
    }
}