using FluentValidation.Results;
using TodoApi.Models.Contracts;

namespace TodoApi.Models.Auth
{
    public class UserAuthRequest : IValidatable, IEmail, IPassword
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public ValidationResult Validate()
        {
            return new ValidationResult([
                new EmailValidator().Validate(this),
                new PasswordValidator().Validate(this)
            ]);
        }
    }
}