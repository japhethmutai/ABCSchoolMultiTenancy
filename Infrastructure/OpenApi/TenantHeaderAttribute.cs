using Infrastructure.Tenancy;

namespace Infrastructure.OpenApi
{
    public class TenantHeaderAttribute() 
        : SwaggerHeaderAttribute(
            TenancyConstants.TenantIdName, 
            "Input your organization name to access this API.", 
            string.Empty, true
        )
    {

    }
}
