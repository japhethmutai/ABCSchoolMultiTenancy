using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Identity.Users.Commands
{
    public class UpdateUserCommand : IRequest<IResponseWrapper>
    {
        public UpdateUserRequest UpdateUserRequest { get; set; }
    }

    public class UpdateUserCommandHandler(IUserService userService) : IRequestHandler<UpdateUserCommand, IResponseWrapper>
    {
        private readonly IUserService _userService = userService;

        public async Task<IResponseWrapper> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userService.UpdateUserAsync(request.UpdateUserRequest);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "User updated successfully.");
        }
    }
}
