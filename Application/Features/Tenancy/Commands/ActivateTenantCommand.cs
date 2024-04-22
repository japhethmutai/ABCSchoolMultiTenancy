using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Tenancy.Commands
{
    public class ActivateTenantCommand : IRequest<IResponseWrapper>
    {
        public string TenantId { get; set; }
    }

    public class ActivateTenantCommandHandler(ITenantService tenantService) : IRequestHandler<ActivateTenantCommand, IResponseWrapper>
    {
        private readonly ITenantService _tenantService = tenantService;

        public async Task<IResponseWrapper> Handle(ActivateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenantId = await _tenantService.ActivateAsync(request.TenantId);
            return await ResponseWrapper<string>.SuccessAsync(data: tenantId, message: "Tenant activated successfully.");
        }
    }
}
