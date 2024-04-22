using System.Net;

namespace Application.Exceptions
{
    public class UnauthorizedException(string message, 
        List<string> errorMessages = default, 
        HttpStatusCode statusCode = HttpStatusCode.Unauthorized) 
        : Exception(message)
    {
        public List<string> ErrorMessages { get; set; } = errorMessages;
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }
}
