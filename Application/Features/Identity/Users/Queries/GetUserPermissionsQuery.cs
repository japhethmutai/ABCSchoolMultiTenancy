using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Identity.Users.Queries
{
    public class GetUserPermissionsQuery : IRequest<IResponseWrapper>
    {
        public string UserId { get; set; }
    }

    public class GetUserPermissionsQueryHandler(IUserService userService) : IRequestHandler<GetUserPermissionsQuery, IResponseWrapper>
    {
        private readonly IUserService _userService = userService;

        public async Task<IResponseWrapper> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _userService.GetPermissionsAsync(request.UserId, cancellationToken);
            return await ResponseWrapper<List<string>>.SuccessAsync(data: permissions);
        }
    }
}
