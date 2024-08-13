using FluentValidation.Results;
using TodoApi.Models.Contracts;

namespace TodoApi.Models.TODO
{
    public class TodoCreateRequest : IValidatable, ITodoCreate
    {
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }

        public ValidationResult Validate()
        {
            return new TodoCreateValidator().Validate(this);
        }
    }
}