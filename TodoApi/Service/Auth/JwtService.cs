using System.Security.Claims;
using TodoApi.Common.Token;
using TodoApi.Repository;
using TodoApi.Models.Auth;
using TodoApi.Service.Identity;
using TodoApi.Common.Extensions;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Constants;

namespace TodoApi.Service.Auth
{
    public class JwtService(RefreshTokenRepository refreshTokenRepository, PrincipalService principalService, IConfiguration configuration)
    {
        private readonly RefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        private readonly PrincipalService _principalService = principalService;
        private readonly IConfiguration _configuration = configuration;

        private static readonly TimeSpan ExpiresInterval = TimeSpan.FromHours(1);

        /// <summary>
        /// Get a new token
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public JWTTokensResponse GetNewToken(ClaimsPrincipal principal)
        {
            return new JWTTokensResponse
            {
                AccessToken = GenerateJwtToken(principal),
                RefreshToken = GenerateRefreshToken(principal)
            };
        }

        public JWTTokensResponse? GetUpdatedToken(ClaimsPrincipal principal, string refreshToken)
        {
            var realToken = _refreshTokenRepository.FetchByToken(refreshToken);
            if (realToken is null)
                return null;
            if (realToken.IsExpired())
                return null;
            if (realToken.UserId != _principalService.GetUserId(principal))
                return null;

            return GetNewToken(principal);
        }

        private string GenerateJwtToken(ClaimsPrincipal principal)
        {
            var userId = _principalService.GetUserId(principal)!;
            var email = _principalService.GetUserEmail(principal)!;

            var privateKey = _configuration.GetSectionValue(SettingsConstants.JwtPrivateKey)!;
            var roles = _principalService.GetRoles(principal);

            var jwtGenerator = new JWTTokenGenerator()
                .SetPrivateKey(privateKey)
                .SetAlgorithm(JWTTokenGenerator.TokenAlgorithm.HmacSha256)
                .SetExpires(DateTime.UtcNow.Add(ExpiresInterval))
                .AddJti()
                .AddSub(userId)
                .AddClaim(ClaimTypes.NameIdentifier, userId)
                .AddClaim(ClaimTypes.Name, email);

            if (roles.IsNullOrEmpty())
            {
                jwtGenerator.AddRangeRole(roles);
            }
            return jwtGenerator.Generate();
        }

        private string GenerateRefreshToken(ClaimsPrincipal principal)
        {
            var userId = _principalService.GetUserId(principal)!;
            var savedToken = SaveRefreshTokenToRepository(userId);
            return savedToken.Token;
        }

        private RefreshToken SaveRefreshTokenToRepository(string userId)
        {
            var token = _refreshTokenRepository.FetchByUserId(userId);
            if (token is null)
            {
                token = new RefreshToken(userId);
                _refreshTokenRepository.Add(token);
            }
            else
            {
                token.UpdateToken();
                _refreshTokenRepository.Update(token);
            }
            return token;
        }
    }
}