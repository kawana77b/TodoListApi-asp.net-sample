using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.Models.Attributes;
using TodoApi.Models.Auth;
using TodoApi.Models.Contracts;

namespace TodoApi.Models.TODO
{
    [Record]
    public record Todo : ITodo, IValidatable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public bool IsDone { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [ForeignKey(nameof(AppUser))]
        public string UserId { get; set; } = string.Empty;

        public FluentValidation.Results.ValidationResult Validate()
        {
            return new TodoValidator().Validate(this);
        }
    }
}