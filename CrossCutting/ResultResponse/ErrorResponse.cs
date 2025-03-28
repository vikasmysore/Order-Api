namespace CrossCutting.ResultResponse
{
    /// <summary>
    /// Base error response. All possible errors inherit from this.
    /// </summary>
    /// <param name="logMessage">Message containing information about the error.</param>
    public class ErrorResponse(string logMessage)
    {
        /// <summary>
        /// Message containing information about the error.
        /// </summary>
        public string LogMessage { get; } = logMessage;

        /// <inheritdoc />
        public override string ToString() => LogMessage;

        /// <summary>
        /// Exposes the <see cref="logMessage"/>.
        /// </summary>
        /// <param name="errorResponse">The instance of <see cref="ErrorResponse"/>.</param>
        /// <returns></returns>
        public static implicit operator string(ErrorResponse errorResponse) => errorResponse.ToString();
    }
}
