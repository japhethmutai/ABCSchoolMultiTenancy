using Application.Features.Tenancy.Models;
using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Tenancy.Queries
{
    public class GetTenantsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetTenantsQueryHandler(ITenantService tenantService) : IRequestHandler<GetTenantsQuery, IResponseWrapper>
    {
        private readonly ITenantService _tenantService = tenantService;

        public async Task<IResponseWrapper> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
        {
            var tenantsInDb = await _tenantService.GetTenantsAsync();
            return await ResponseWrapper<List<TenantDto>>.SuccessAsync(data: tenantsInDb);
        }
    }
}
