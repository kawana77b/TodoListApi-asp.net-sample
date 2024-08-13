using System.Text.Json.Serialization;

namespace TodoApi.Models.Auth
{
    public class JWTTokensResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}