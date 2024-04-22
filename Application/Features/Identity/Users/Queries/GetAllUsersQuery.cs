using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Identity.Users.Queries
{
    public class GetAllUsersQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetAllUsersQueryHandler(IUserService userService) : IRequestHandler<GetAllUsersQuery, IResponseWrapper>
    {
        private readonly IUserService _userService = userService;

        public async Task<IResponseWrapper> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.GetUsersAsync(cancellationToken);
            return await ResponseWrapper<List<UserDto>>.SuccessAsync(data: users);
        }
    }
}
