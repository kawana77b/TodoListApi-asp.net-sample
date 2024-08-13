using FluentValidation.Results;
using TodoApi.Models.Contracts;

namespace TodoApi.Models.Auth
{
    public partial class UserDeleteRequest : IValidatable, IEmail
    {
        public string Email { get; set; } = string.Empty;

        public ValidationResult Validate()
        {
            return new EmailValidator().Validate(this);
        }
    }
}