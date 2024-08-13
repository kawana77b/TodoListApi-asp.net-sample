namespace TodoApi.Common.Contracts
{
    /// <summary>
    /// Expression with respect to results
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TError"></typeparam>
    public interface IResultData<TValue, TError>
    {
        /// <summary>
        /// Whether it is successful
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Value when successful
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Error when failed
        /// </summary>
        TError Error { get; }
    }
}