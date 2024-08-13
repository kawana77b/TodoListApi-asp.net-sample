namespace TodoApi.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Gets the value of the specified key in the configuration. If the value is null, an error occurs.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetSectionValue(this IConfiguration @this, string key) => @this.GetSection(key).Value ?? throw new ArgumentNullException($"Configuration: {key} is null");
    }
}