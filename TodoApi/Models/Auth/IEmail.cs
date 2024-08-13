using FluentValidation;

namespace TodoApi.Models.Auth
{
    public interface IEmail
    {
        string Email { get; set; }
    }

    public class EmailValidator : AbstractValidator<IEmail>
    {
        public EmailValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(250);
        }
    }
}