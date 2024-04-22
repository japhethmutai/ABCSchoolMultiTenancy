using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Identity.Roles.Commands
{
    public class CreateRoleCommand : IRequest<IResponseWrapper>
    {
        public CreateRoleRequest RoleRequest { get; set; }
    }

    public class CreateRoleCommandHandler(IRoleService roleService) : IRequestHandler<CreateRoleCommand, IResponseWrapper>
    {
        private readonly IRoleService _roleService = roleService;
        public async Task<IResponseWrapper> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var newRole = await _roleService.CreateAsync(request.RoleRequest);
            return await ResponseWrapper<string>.SuccessAsync(data: newRole, message: "Role created successfully.");
        }
    }
}
