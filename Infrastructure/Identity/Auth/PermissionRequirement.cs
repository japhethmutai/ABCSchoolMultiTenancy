using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity.Auth
{
    internal class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; set; } = permission;
    }
}
