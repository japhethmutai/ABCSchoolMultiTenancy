using Application.Features.Identity.Tokens;
using Application.Features.Identity.Tokens.Queries;
using Azure.Core;
using Infrastructure.OpenApi;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TokensController : BaseApiController
    {
        private readonly ILogger<TokensController> _logger;

        public TokensController(ILogger<TokensController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [TenantHeader]
        [OpenApiOperation("Used to obtain jwt for login.")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest tokenRequest)
        {
            if (HttpContext.Request.Headers.TryGetValue(TenancyConstants.TenantIdName, out var customHeaderValue))
                _logger.LogInformation($"Tenant parameter passed is: {customHeaderValue}");
            _logger.LogInformation("User authentication process started...");

            var response = await Sender.Send(new GetTokenQuery { TokenRequest = tokenRequest });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [TenantHeader]
        [OpenApiOperation("Used to obtain new jwt for login via refresh token.")]
        public async Task<IActionResult> GetRefreshTokenAsync([FromBody] RefreshTokenRequest refreshToken)
        {
            var response = await Sender.Send(new GetRefreshTokenQuery { RefreshToken = refreshToken });

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
