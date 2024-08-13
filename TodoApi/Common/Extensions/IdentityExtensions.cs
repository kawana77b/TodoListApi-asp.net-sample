using Microsoft.AspNetCore.Identity;

namespace TodoApi.Common.Extensions
{
    public static class IdentityExtensions
    {
        /// <summary>
        /// Gets the error descriptions of the IdentityResult.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetErrorsDescriptions(this IdentityResult @this) => @this.Errors.Select(x => x.Description);
    }
}