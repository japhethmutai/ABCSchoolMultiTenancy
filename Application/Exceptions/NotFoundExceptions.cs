using System.Net;

namespace Application.Exceptions
{
    public class NotFoundExceptions(string message,
        List<string> errorMessages = default,
        HttpStatusCode statusCode = HttpStatusCode.NotFound) : Exception(message)
    {
        public List<string> ErrorMessages { get; set; } = errorMessages;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }
}
