namespace TodoApi.Common.Contracts
{
    /// <summary>
    /// Indicates that a token is to be generated
    /// </summary>
    public interface ITokenGenerator
    {
        /// <summary>
        /// Generate a token
        /// </summary>
        /// <returns></returns>
        public string Generate();
    }
}