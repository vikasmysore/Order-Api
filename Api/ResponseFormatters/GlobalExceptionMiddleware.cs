namespace Api.ResponseFormatters
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Continue processing the HTTP request
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Handle the exception and return a formatted error response
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception for internal tracking
            _logger.LogError(exception, "An unexpected error occurred.");

            // Set default status code and message
            var statusCode = StatusCodes.Status500InternalServerError; // Internal Server Error by default
            var message = "An unexpected error occurred.";
            //var details = exception.Message;

            // Customize the status code and message for specific exception types
            if (exception is UnauthorizedAccessException)
            {
                statusCode = StatusCodes.Status401Unauthorized;
                message = "You are not authorized to access this resource.";
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                message = "The requested resource was not found.";
            }
            else if (exception is ArgumentException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                message = "The provided data is invalid.";
            }
            else if (exception is Exception)
            {
                if (exception.Message.Contains("Order Not Found"))
                {
                    statusCode = StatusCodes.Status404NotFound;
                    message = "The requested order was not found.";
                }
                else if (exception.Message.Contains("No orders to show."))
                {
                    statusCode = StatusCodes.Status404NotFound;
                    message = "No orders were found for your account.";
                }
                else
                {
                    statusCode = StatusCodes.Status400BadRequest;
                    message = "The provided data is invalid.";
                }
            }

            // Set the response content type and status code
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Prepare the response
            var errorResponse = new
            {
                statusCode = statusCode,
                message = message
                //details = details
            };

            return context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
