using System.Text;
using TodoApi.Common.Extensions;

namespace TodoApi.Common.Token
{
    /// <summary>
    /// Indicates private key information
    /// </summary>
    public record PrivateKey
    {
        public static PrivateKey Create(string value) => new PrivateKey { Value = value };

        public static PrivateKey FromConfiguration(IConfiguration configuration, string key) => Create(configuration.GetSectionValue(key)!);

        public static PrivateKey FromEnvironment(string key) => Create(Environment.GetEnvironmentVariable(key)!);

        public string Value { get; init; } = string.Empty;

        public byte[] Bytes() => Encoding.UTF8.GetBytes(Value);

        public override string ToString() => Value;
    }
}