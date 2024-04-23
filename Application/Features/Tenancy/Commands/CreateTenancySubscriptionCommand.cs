using Application.Features.Tenancy.Models;
using Application.Models.Wrapper;
using MediatR;

namespace Application.Features.Tenancy.Commands
{
    public class CreateTenancySubscriptionCommand : IRequest<IResponseWrapper>
    {
        public CreateTenancySubscriptionRequest TenancySubscriptionRequest { get; set; }
    }

    public class CreateTenancySubscriptionCommandHandler(
        ITenancySubscriptionService tenancySubscriptionService) 
        : IRequestHandler<CreateTenancySubscriptionCommand, IResponseWrapper>
    {
        private readonly ITenancySubscriptionService _tenancySubscriptionService = tenancySubscriptionService;

        public async Task<IResponseWrapper> Handle(CreateTenancySubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscriptionId = await _tenancySubscriptionService
                .CreateTenancySubscriptionAsync(request.TenancySubscriptionRequest, cancellationToken);
            return await ResponseWrapper<int>.SuccessAsync(data: subscriptionId, message: "Tenancy subscription created successfully.");
        }
    }
}
