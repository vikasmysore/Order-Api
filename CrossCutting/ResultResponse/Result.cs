namespace CrossCutting.ResultResponse
{
    /// <summary>
    /// A wrapper that encapsulates a method's response so that
    /// its implementation can return error information in case
    /// the operation fails.
    /// </summary>
    /// <typeparam name="T">The type of the actual response of the operation.</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// The content of the response.
        /// </summary>
        public T? Content { get; init; }

        /// <summary>
        /// Information about the error that occurred when performing the operation.
        /// </summary>
        public ErrorResponse? ErrorResponse { get; init; }

        /// <summary>
        /// If true, <see cref="Content"/> will be set to null and <see cref="ErrorResponse"/> will have information on what the error was.
        /// If false, <see cref="Content"/> will contain the operation's response and no <see cref="ErrorResponse"/> will be set.
        /// </summary>
        public bool IsError { get; init; }

        public static implicit operator T(Result<T> result) => result.Content ?? throw new NullReferenceException();

        public static implicit operator Result<T>(T content) => new(content);

        public static implicit operator Result<T>(ErrorResponse errorResponse) => new(errorResponse);

        /// <summary>
        /// The constructor for a success response.
        /// </summary>
        /// <param name="content">The response to be returned.</param>
        public Result(T content)
        {
            Content = content;
            IsError = false;
        }

        /// <summary>
        /// The constructor for a failure response.
        /// </summary>
        /// <param name="errorResponse">An <see cref="ErrorResponse"/> detailing the problem.</param>
        public Result(ErrorResponse errorResponse)
        {
            ErrorResponse = errorResponse;
            IsError = true;
        }
    }
}
