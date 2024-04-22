using Application.Features.Tenancy.Models;
using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Tenancy.Commands
{
    public class UpdateTenantSubscriptionCommand : IRequest<IResponseWrapper>
    {
        public UpdateTenantSubscriptionRequest TenantRequest { get; set; }
    }

    public class UpdateTenantSubscriptionCommandHandler(ITenantService tenantService) 
        : IRequestHandler<UpdateTenantSubscriptionCommand, IResponseWrapper>
    {
        private readonly ITenantService _tenantService = tenantService;

        public async Task<IResponseWrapper> Handle(UpdateTenantSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var tenantId = await _tenantService
                .UpgradeSubscriptionAsync(request.TenantRequest.TenantId, request.TenantRequest.NewExpiryDate);

            return await ResponseWrapper<string>.SuccessAsync(data: tenantId, message: "Tenant subscription updated successfully.");
        }
    }
}
