using Application.Exceptions;
using Application.Features.Identity.Users;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Users
{
    public class UserService(
        UserManager<ApplicationUser> userManager, 
        RoleManager<ApplicationRole> roleManager, 
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context, ITenantInfo tenant) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ApplicationDbContext _context = context;
        private readonly ITenantInfo _tenant = tenant;

        public async Task<string> ActivateOrDeactivateAsync(string userId, bool activation)
        {
            var userInDb = await GetUserAsync(userId);

            userInDb.IsAtive = activation;

            await _userManager.UpdateAsync(userInDb);
            return userInDb.Id;
        }

        public async Task<string> AssignRolesAsync(string userId, UserRolesRequest request)
        {
            var userInDb = await GetUserAsync(userId);

            if (await _userManager.IsInRoleAsync(userInDb, RoleConstants.Admin)
                && request.UserRoles.Any(ur => !ur.IsAssigned && ur.Name == RoleConstants.Admin))
            {
                var adminUsersCount = (await _userManager.GetUsersInRoleAsync(RoleConstants.Admin)).Count;
                if (userInDb.Email == TenancyConstants.Root.Email)
                {
                    if (_tenant.Id == TenancyConstants.Root.Id)
                    {
                        throw new ConflictException("Not allowed to remove Admin Role for a Root Tenant user.");
                    }
                }
                else if (adminUsersCount <= 2)
                {
                    throw new ConflictException("An organization should have at least 2 admin users.");
                }
            }

            foreach (var userRole in request.UserRoles)
            {
                if (await _roleManager.FindByIdAsync(userRole.RoleId) is not null)
                {
                    if (userRole.IsAssigned)
                    {
                        if (!await _userManager.IsInRoleAsync(userInDb, userRole.Name))
                        {
                            await _userManager.AddToRoleAsync(userInDb, userRole.Name);
                        }
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(userInDb, userRole.Name);
                    }
                }
            }

            return "Roles Assigned Successfully.";
        }

        public async Task<string> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var userInDb = await GetUserAsync(request.UserId);

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                throw new ConflictException("Passwords do not match.");
            }

            var result = await _userManager.ChangePasswordAsync(userInDb, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to change password.", GetIdentityResultErrorDescriptions(result));
            }
            return userInDb.Id;
        }

        public async Task<string> CreateUserAsync(CreateUserRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new ConflictException("Passwords do not match.");
            }

            //if (await IsEmailTakenAsync(request.Email))
            //{
            //    throw new ConflictException("Email already taken.");
            //}

            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                IsAtive = request.IsAtive,
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to create user.", GetIdentityResultErrorDescriptions(result));
            }
            return newUser.Id;
        }

        public async Task<string> DeleteUserAsync(string userId)
        {
            var userInDb = await GetUserAsync(userId);

            var result = await _userManager.DeleteAsync(userInDb);

            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to delete user.", GetIdentityResultErrorDescriptions(result));
            }
            return userInDb.Id;
        }

        public async Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var userInDb = await GetUserAsync(userId, cancellationToken);

            return userInDb.Adapt<UserDto>();
        }

        public async Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken cancellationToken)
        {
            var userRoles = new List<UserRoleDto>();

            var userInDb = await GetUserAsync(userId, cancellationToken);
            var roles = await _roleManager
                .Roles
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach (var role in roles)
            {
                userRoles.Add(new UserRoleDto
                {
                    RoleId = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    IsAssigned = await _userManager.IsInRoleAsync(userInDb, role.Name),
                });
            }

            return userRoles;
        }

        public async Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
        {
            var usersInDb = await _userManager
                .Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return usersInDb.Adapt<List<UserDto>>();
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return (await _userManager.FindByEmailAsync(email)) is not null;

            //if ((await _userManager.FindByEmailAsync(email)) is not null)
            //{
            //    return true;
            //}
            //return false;
        }

        public async Task<string> UpdateUserAsync(UpdateUserRequest request)
        {
            var userInDb = await GetUserAsync(request.Id);

            userInDb.FirstName = request.FirstName;
            userInDb.LastName = request.LastName;
            userInDb.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(userInDb);

            if (!result.Succeeded)
            {
                throw new IdentityException("Failed to update user.", GetIdentityResultErrorDescriptions(result));
            }
            await _signInManager.RefreshSignInAsync(userInDb);
            return userInDb.Id;
        }
        
        public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
        {
            var userInDb = await GetUserAsync(userId, cancellationToken);
            var userRoles = await _userManager.GetRolesAsync(userInDb);

            var permissions = new List<string>();

            foreach (var role in await _roleManager
                .Roles
                .Where(r => userRoles.Contains(r.Name))
                .ToListAsync(cancellationToken))
            {
                permissions.AddRange(await _context
                    .RoleClaims
                    .Where(rc => rc.RoleId == role.Id && rc.ClaimType == ClaimConstants.Permission)
                    .Select(rc => rc.ClaimValue)
                    .ToListAsync(cancellationToken));
            }

            // Verify if it is necessary to check distinctness - .Distinct().ToList()
            return permissions.Distinct().ToList();
        }

        public async Task<bool> IsPermissionAssignedAsync(string userId, string permission, CancellationToken ct)
            => (await GetPermissionsAsync(userId, ct)).Contains(permission);

        private async Task<ApplicationUser> GetUserAsync(string userId, CancellationToken ct = default)
            => await _userManager
                .Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(ct) ?? throw new NotFoundExceptions("User does not exists.");

        private List<string> GetIdentityResultErrorDescriptions(IdentityResult identityResult)
        {
            var errorDescriptions = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                errorDescriptions.Add(error.Description);
            }

            return errorDescriptions;
        }
    }
}
