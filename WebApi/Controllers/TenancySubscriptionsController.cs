using Application.Features.Tenancy.Commands;
using Application.Features.Tenancy.Models;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenancySubscriptionsController : BaseApiController
    {
        [HttpPost]
        [ShouldHavePermission(SchoolAction.Create, SchoolFeature.TenancySubscription)]
        public async Task<IActionResult> CreateTenancySubscriptionAsync(CreateTenancySubscriptionRequest request)
        {
            var response = await Sender.Send(new CreateTenancySubscriptionCommand { TenancySubscriptionRequest = request });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
