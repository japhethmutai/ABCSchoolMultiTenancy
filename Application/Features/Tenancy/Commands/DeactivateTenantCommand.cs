using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Tenancy.Commands
{
    public class DeactivateTenantCommand : IRequest<IResponseWrapper>
    {
        public string TenantId { get; set; }
    }

    public class DeactivateTenantCommandHandler(ITenantService tenantService) : IRequestHandler<DeactivateTenantCommand, IResponseWrapper>
    {
        private readonly ITenantService _tenantService = tenantService;

        public async Task<IResponseWrapper> Handle(DeactivateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenantId = await _tenantService.DeactivateAsync(request.TenantId);
            return await ResponseWrapper<string>.SuccessAsync(data: tenantId, message: "Tenant deactivated successfully.");
        }
    }
}
