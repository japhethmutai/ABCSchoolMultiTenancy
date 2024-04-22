namespace Application.Features.Identity.Roles
{
    public class UpdateRolePermissionsRequest
    {
        public string RoleId { get; set; }
        public List<string> Permissions { get; set; }
    }
}
