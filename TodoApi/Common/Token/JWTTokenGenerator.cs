using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using TodoApi.Common.Contracts;

namespace TodoApi.Common.Token
{
    public sealed class JWTTokenGenerator : ITokenGenerator
    {
        public enum TokenAlgorithm
        {
            HmacSha256,
        }

        private static string GetAlgorithmName(TokenAlgorithm algorithm)
        {
            return algorithm switch
            {
                TokenAlgorithm.HmacSha256 => SecurityAlgorithms.HmacSha256,
                _ => throw new ArgumentException("Invalid algorithm"),
            };
        }

        public readonly List<Claim> _claims = [];

        public IEnumerable<Claim> Claims => _claims;

        public TokenAlgorithm Algorithm { get; private set; } = TokenAlgorithm.HmacSha256;

        [JsonIgnore]
        public PrivateKey? PrivateKey { get; private set; }

        public DateTime? NotBefore { get; private set; }

        public DateTime? IssuedAt { get; private set; }

        public DateTime Expires { get; private set; } = DateTime.UtcNow.AddMonths(1);

        public string? Issuer { get; private set; }

        public string? Audience { get; private set; }

        public JWTTokenGenerator()
        {
        }

        public string Generate()
        {
            var tokenDescriptor = GenerateDescriptor();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        #region GenerateMethods

        private SecurityTokenDescriptor GenerateDescriptor()
        {
            if (_claims.Count == 0) throw new InvalidOperationException("Claims is empty");
            if (PrivateKey is null) throw new InvalidOperationException("PrivateKey is not set");

            var tokenDescriptor = new SecurityTokenDescriptor();
            tokenDescriptor.SigningCredentials = CreateCredentials();
            tokenDescriptor.Subject = CreateSubject();
            tokenDescriptor.Expires = Expires;

            if (NotBefore != null) tokenDescriptor.NotBefore = NotBefore;
            if (Issuer != null) tokenDescriptor.Issuer = Issuer;
            if (Audience != null) tokenDescriptor.Audience = Audience;

            return tokenDescriptor;
        }

        private SigningCredentials CreateCredentials()
        {
            if (PrivateKey is null) throw new InvalidOperationException("PrivateKey is not set");

            var securityKey = new SymmetricSecurityKey(PrivateKey.Bytes());
            return new SigningCredentials(securityKey, GetAlgorithmName(Algorithm));
        }

        private ClaimsIdentity CreateSubject()
        {
            return new ClaimsIdentity(_claims.ToArray());
        }

        #endregion GenerateMethods

        #region Setters

        public JWTTokenGenerator SetAlgorithm(TokenAlgorithm algorithm)
        {
            Algorithm = algorithm;
            return this;
        }

        public JWTTokenGenerator SetPrivateKey(string privateKey)
        {
            PrivateKey = PrivateKey.Create(privateKey);
            return this;
        }

        public JWTTokenGenerator SetExpires(DateTime expires)
        {
            Expires = expires;
            return this;
        }

        public JWTTokenGenerator SetNotBefore(DateTime notBefore)
        {
            NotBefore = notBefore;
            return this;
        }

        public JWTTokenGenerator SetIssuedAt(DateTime issuedAt)
        {
            IssuedAt = issuedAt;
            return this;
        }

        public JWTTokenGenerator SetIssuer(string issuer)
        {
            Issuer = issuer;
            return this;
        }

        public JWTTokenGenerator SetAudience(string audience)
        {
            Audience = audience;
            return this;
        }

        /// <summary>
        /// Determines the attributes of jti. If the string is empty, takes the new GUID as value.
        /// </summary>
        /// <param name="jti"></param>
        /// <returns></returns>
        public JWTTokenGenerator AddJti(string jti = "")
        {
            var value = string.IsNullOrWhiteSpace(jti) ? Guid.NewGuid().ToString() : jti;
            AddClaim(JwtRegisteredClaimNames.Jti, value);
            return this;
        }

        /// <summary>
        /// Determines the attributes of sub.
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public JWTTokenGenerator AddSub(string sub)
        {
            if (string.IsNullOrWhiteSpace(sub)) throw new ArgumentNullException(nameof(sub));

            AddClaim(JwtRegisteredClaimNames.Sub, sub);
            return this;
        }

        public JWTTokenGenerator AddNameIdentifier(string nameIdentifier)
        {
            AddClaim(ClaimTypes.NameIdentifier, nameIdentifier);
            return this;
        }

        public JWTTokenGenerator AddName(string name)
        {
            AddClaim(ClaimTypes.Name, name);
            return this;
        }

        public JWTTokenGenerator AddEmail(string email)
        {
            AddClaim(ClaimTypes.Email, email);
            return this;
        }

        public JWTTokenGenerator AddRole(IdentityRole role)
        {
            if (role.Name is null) throw new ArgumentNullException(nameof(role));

            AddRole(role.Name);
            return this;
        }

        public JWTTokenGenerator AddRole(string role)
        {
            AddClaim(ClaimTypes.Role, role);
            return this;
        }

        public JWTTokenGenerator AddRangeRole(IEnumerable<string> roles)
        {
            foreach (var role in roles)
                AddRole(role);
            return this;
        }

        public JWTTokenGenerator AddRangeRole(IEnumerable<IdentityRole> roles)
        {
            foreach (var role in roles)
                AddRole(role);
            return this;
        }

        public JWTTokenGenerator AddClaim(Claim claim)
        {
            _claims.Add(claim);
            return this;
        }

        public JWTTokenGenerator AddClaim(string type, string value)
        {
            _claims.Add(new Claim(type, value));
            return this;
        }

        public JWTTokenGenerator AddRangeClaim(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
                AddClaim(claim);
            return this;
        }

        #endregion Setters

        /// <summary>
        /// <c>Debug.WriteLine</c> with the state of this object in JSON format. However, some parts may be excluded
        /// </summary>
        [Conditional("DEBUG")]
        public void Dump()
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });
            Debug.WriteLine(json);
        }
    }
}