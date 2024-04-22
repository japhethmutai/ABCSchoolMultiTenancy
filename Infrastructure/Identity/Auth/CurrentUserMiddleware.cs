using Application.Features.Identity.Users;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity.Auth
{
    public class CurrentUserMiddleware(ICurrentUserService currentUserService) : IMiddleware
    {
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _currentUserService.SetCurrentUser(context.User);
            await next(context);
        }
    }
}
