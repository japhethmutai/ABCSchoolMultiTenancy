using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
