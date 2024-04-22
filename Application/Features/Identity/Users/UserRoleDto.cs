namespace Application.Features.Identity.Users
{
    public class UserRoleDto
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAssigned { get; set; }
    }

    public class UserRolesRequest
    {
        public List<UserRoleDto> UserRoles { get; set; }
    }
}
