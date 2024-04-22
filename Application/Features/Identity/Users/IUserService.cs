namespace Application.Features.Identity.Users
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(CreateUserRequest request);
        Task<string> UpdateUserAsync(UpdateUserRequest request);
        Task<string> DeleteUserAsync(string userId);
        Task<string> ActivateOrDeactivateAsync(string userId, bool activation);
        Task<string> ChangePasswordAsync(ChangePasswordRequest request);
        Task<string> AssignRolesAsync(string userId, UserRolesRequest request);

        Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken);
        Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
        Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken cancellationToken);
        Task<bool> IsEmailTakenAsync(string email);
        Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken);
        Task<bool> IsPermissionAssignedAsync(string userId, string permission, CancellationToken cancellationToken = default);
    }
}
