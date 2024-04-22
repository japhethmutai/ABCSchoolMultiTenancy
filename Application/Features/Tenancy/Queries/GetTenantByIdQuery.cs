using Application.Features.Tenancy.Models;
using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Tenancy.Queries
{
    public class GetTenantByIdQuery : IRequest<IResponseWrapper>
    {
        public string TenantId { get; set; }
    }

    public class GetTenantByIdQueryHandler(ITenantService tenantService) : IRequestHandler<GetTenantByIdQuery, IResponseWrapper>
    {
        private readonly ITenantService _tenantService = tenantService;

        public async Task<IResponseWrapper> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            var tenantInDb = await _tenantService.GetTenantByIdAsync(request.TenantId);

            if (tenantInDb is null)
            {
                return await ResponseWrapper<string>.FailAsync(message: "Tenant does not exist.");
            }
            return await ResponseWrapper<TenantDto>.SuccessAsync(data: tenantInDb);
        }
    }
}
