using FluentValidation;
using FluentValidation.Results;
using System.Text.Json.Serialization;
using TodoApi.Models.Contracts;

namespace TodoApi.Models.Auth
{
    public class RefreshTokenRequest : IValidatable
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        public ValidationResult Validate()
        {
            return new RefreshTokenRequestValidator().Validate(this);
        }
    }

    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}