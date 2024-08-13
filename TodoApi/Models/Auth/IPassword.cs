using FluentValidation;

namespace TodoApi.Models.Auth
{
    public interface IPassword
    {
        public string Password { get; set; }
    }

    public class PasswordValidator : AbstractValidator<IPassword>
    {
        public PasswordValidator()
        {
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(250);
        }
    }
}