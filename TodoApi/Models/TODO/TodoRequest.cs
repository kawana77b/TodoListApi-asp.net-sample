using FluentValidation.Results;
using TodoApi.Models.Contracts;

namespace TodoApi.Models.TODO
{
    public class TodoRequest : IValidatable, ITodo
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool IsDone { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UserId { get; set; } = string.Empty;

        public ValidationResult Validate()
        {
            return new TodoValidator().Validate(this);
        }
    }
}