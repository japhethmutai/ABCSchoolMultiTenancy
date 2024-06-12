using Application.Features.Schools;
using Application.Features.Teachers;
using Infrastructure.Identity;
using Infrastructure.OpenApi;
using Infrastructure.Persistence;
using Infrastructure.Schools;
using Infrastructure.Teachers;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddMultitenancyServices(configuration)
                .AddPersistenceService(configuration)
                .AddIdentityServices()
                .AddPermissions()
                .AddJwtAuthentication()
                .AddOpenApiDocumentation(configuration)
                .AddScoped<ISchoolService, SchoolService>()
                .AddScoped<ITeacherService, TeacherService>();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            return app
                .UseAuthentication()
                .UseCurrentUser()
                .UseMultitenancy()
                .UseAuthorization()
                .UseOpenApiDocumentation();
        }
    }
}
