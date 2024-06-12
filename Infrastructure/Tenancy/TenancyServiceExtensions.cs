using Application.Features.Tenancy;
using Infrastructure.Identity.Constants;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Tenancy
{
    internal static class TenancyServiceExtensions
    {
        internal static IServiceCollection AddMultitenancyServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<TenantDbContext>((p, m) =>
                {
                    // TODO: We should probably add specific dbprovider/connectionstring setting for the tenantDb with a fallback to the main databasesettings
                    var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                    m.UseDatabase(databaseSettings.DBProvider, databaseSettings.ConnectionString);
                })
                //.AddDbContext<TenantDbContext>(options => options
                //    .UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
                //.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddMultiTenant<ABCSchoolTenantInfo>()
                .WithHeaderStrategy(TenancyConstants.TenantIdName)
                .WithClaimStrategy(ClaimConstants.Tenant)
                .WithCustomQueryStringStrategy(TenancyConstants.TenantIdName)
                .WithEFCoreStore<TenantDbContext, ABCSchoolTenantInfo>()
                .Services
                .AddScoped<ITenantService, TenantService>()
                .AddScoped<ITenancySubscriptionService, TenancySubscriptionService>();
        }

        internal static IApplicationBuilder UseMultitenancy(this IApplicationBuilder app)
        {
            return app
                .UseMultiTenant();
        }

        private static FinbuckleMultiTenantBuilder<ABCSchoolTenantInfo> WithCustomQueryStringStrategy(
            this FinbuckleMultiTenantBuilder<ABCSchoolTenantInfo> builder, string customQueryString)
        {
            return builder
                .WithDelegateStrategy(context =>
                {
                    if (context is not HttpContext httpContext)
                    {
                        return Task.FromResult((string)null);
                    }

                    var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("TenantResolution");
                    logger.LogInformation("Query string: {QueryString}", httpContext.Request.QueryString);
                    logger.LogInformation($"Custom Query string: {customQueryString}");

                    httpContext.Request.Query.TryGetValue(customQueryString, out StringValues tenantIdParam);

                    var tenantId = tenantIdParam.ToString();

                    logger.LogInformation($"Tenant ID from query string: {tenantId}");

                    return Task.FromResult(tenantId);
                });
        }
    }
}
