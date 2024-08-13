using FluentValidation.Results;

namespace TodoApi.Models.Contracts
{
    /// <summary>
    /// This model indicates that the value can be verified
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Validate against this model
        /// </summary>
        /// <returns></returns>
        ValidationResult Validate();
    }
}