using FluentValidation;

namespace TodoApi.Models.TODO
{
    public interface ITodoCreate
    {
        public string Title { get; set; }

        public bool IsDone { get; set; }
    }

    public class TodoCreateValidator : AbstractValidator<ITodoCreate>
    {
        public TodoCreateValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(1).MaximumLength(100);
            RuleFor(x => x.IsDone).NotNull();
        }
    }
}