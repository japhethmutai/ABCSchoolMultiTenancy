using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Identity.Roles.Commands
{
    public class DeleteRoleCommand : IRequest<IResponseWrapper>
    {
        public string RoleId { get; set; }
    }

    public class DeleteRoleCommandHandler(IRoleService roleService) : IRequestHandler<DeleteRoleCommand, IResponseWrapper>
    {
        private readonly IRoleService _roleService = roleService;
        public async Task<IResponseWrapper> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var deletedRole = await _roleService.DeleteAsync(request.RoleId);
            return await ResponseWrapper<string>.SuccessAsync(data: deletedRole, message: "Role deleted successfully.");
        }
    }
}
