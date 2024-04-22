using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Identity.Users.Commands
{
    public class CreateUserCommand : IRequest<IResponseWrapper>
    {
        public CreateUserRequest CreateUserRequest { get; set; }
    }

    public class CreateUserCommandHandler(IUserService userService) : IRequestHandler<CreateUserCommand, IResponseWrapper>
    {
        private readonly IUserService _userService = userService;

        public async Task<IResponseWrapper> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userService.CreateUserAsync(request.CreateUserRequest);
            return await ResponseWrapper<string>.SuccessAsync(data: userId, message: "User created successfully.");
        }
    }
}
