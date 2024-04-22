using System.Net;

namespace Application.Exceptions
{
    public class ForbiddenException(string message,
        List<string> errorMessages = default,
        HttpStatusCode statusCode = HttpStatusCode.Forbidden) : Exception(message)
    {
        public List<string> ErrorMessages { get; set; } = errorMessages;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }
}
