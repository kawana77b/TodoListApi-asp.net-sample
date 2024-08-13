using FluentValidation;

namespace TodoApi.Models.TODO
{
    public interface ITodo : ITodoCreate
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UserId { get; set; }
    }

    public class TodoValidator : AbstractValidator<ITodo>
    {
        public TodoValidator() : base()
        {
            Include(new TodoCreateValidator());
            RuleFor(x => x.UserId).NotNull();
        }
    }
}