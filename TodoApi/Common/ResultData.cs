using TodoApi.Common.Contracts;

namespace TodoApi.Common
{
    /// <summary>
    /// Generic utilities showing results
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TError"></typeparam>
    public class ResultData<TValue, TError> : IResultData<TValue, TError>
    {
        /// <summary>
        /// Representing Success
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ResultData<TValue, TError> Success(TValue value) => new() { IsSuccess = true, Value = value };

        /// <summary>
        /// Representing Failure
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ResultData<TValue, TError> Failure(TError value) => new() { IsSuccess = false, Error = value };

        public bool IsSuccess { get; init; }

        public TValue Value { get; init; } = default!;

        public TError Error { get; init; } = default!;
    }
}