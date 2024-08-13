using System.Security.Cryptography;
using TodoApi.Common.Contracts;

namespace TodoApi.Common.Token
{
    public class RefreshTokenGenerator : ITokenGenerator
    {
        public int TokenLength { get; init; }

        public RefreshTokenGenerator(int tokenLength = 128)
        {
            TokenLength = tokenLength;
        }

        public string Generate()
        {
            var randBytes = new byte[TokenLength];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randBytes);
            return Convert.ToBase64String(randBytes);
        }
    }
}