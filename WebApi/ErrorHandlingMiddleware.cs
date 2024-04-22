using Application.Exceptions;
using Application.Models.Wrapper;
using System.Net;
using System.Text.Json;

namespace WebApi
{
    public class ErrorHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";

                var responseWrapper = await ResponseWrapper.FailAsync(ex.Message);

                response.StatusCode = ex switch
                {
                    ConflictException ce => (int)ce.StatusCode,
                    NotFoundExceptions nfe => (int)nfe.StatusCode,
                    ForbiddenException fe => (int)fe.StatusCode,
                    IdentityException ie => (int)ie.StatusCode,
                    UnauthorizedException ue => (int)ue.StatusCode,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var result = JsonSerializer.Serialize(responseWrapper);

                await response.WriteAsync(result);
            }
        }
    }
}
