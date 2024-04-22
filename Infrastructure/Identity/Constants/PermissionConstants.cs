using System.Collections.ObjectModel;

namespace Infrastructure.Identity.Constants
{
    public static class SchoolAction
    {
        public const string View = nameof(View);
        public const string Create = nameof(Create);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
        public const string UpgradeSubscription = nameof(UpgradeSubscription);
    }

    public static class SchoolFeature
    {
        public const string Tenants = nameof(Tenants);
        public const string Users = nameof(Users);
        public const string UserRoles = nameof(UserRoles);
        public const string Roles = nameof(Roles);
        public const string RoleClaims = nameof(RoleClaims);
        public const string Schools = nameof(Schools);
    }

    public record SchoolPermission(string Description, string Action, string Feature, bool IsBasic = false, bool IsRoot = false)
    {
        public string Name => NameFor(Action, Feature);

        // Permission.Feature.Action
        public static string NameFor(string action, string feature) => $"Permission.{feature}.{action}";
    }

    public static class SchoolPermissions
    {
        private static readonly SchoolPermission[] _allPermissions =
        [
            new SchoolPermission("View Users", SchoolAction.View, SchoolFeature.Users),
            new SchoolPermission("Create User", SchoolAction.Create, SchoolFeature.Users),
            new SchoolPermission("Update User", SchoolAction.Update, SchoolFeature.Users),
            new SchoolPermission("Delete User", SchoolAction.Delete, SchoolFeature.Users),
                
            
            new SchoolPermission("View UserRoles", SchoolAction.View, SchoolFeature.UserRoles),
            new SchoolPermission("Update UserRole", SchoolAction.Update, SchoolFeature.UserRoles),

            new SchoolPermission("View Roles", SchoolAction.View, SchoolFeature.Roles),
            new SchoolPermission("Create Role", SchoolAction.Create, SchoolFeature.Roles),
            new SchoolPermission("Update Role", SchoolAction.Update, SchoolFeature.Roles),
            new SchoolPermission("Delete Role", SchoolAction.Delete, SchoolFeature.Roles),

            new SchoolPermission("View RoleClaims/Permissions", SchoolAction.View, SchoolFeature.RoleClaims),
            new SchoolPermission("Update RoleClaims/Permissions", SchoolAction.Update, SchoolFeature.RoleClaims),

            new SchoolPermission("View Schools", SchoolAction.View, SchoolFeature.Schools, IsBasic: true),
            new SchoolPermission("Create School", SchoolAction.Create, SchoolFeature.Schools),
            new SchoolPermission("Update School", SchoolAction.Update, SchoolFeature.Schools),
            new SchoolPermission("Delete School", SchoolAction.Delete, SchoolFeature.Schools),
            
            new SchoolPermission("View Tenants", SchoolAction.View, SchoolFeature.Tenants, IsRoot: true),
            new SchoolPermission("Create Tenant", SchoolAction.Create, SchoolFeature.Tenants, IsRoot: true),
            new SchoolPermission("Update Tenant", SchoolAction.Update, SchoolFeature.Tenants, IsRoot: true),
            new SchoolPermission("Upgrade Tenant Subscription", SchoolAction.UpgradeSubscription, SchoolFeature.Tenants, IsRoot: true),
        ];

        public static IReadOnlyList<SchoolPermission> All { get; } = 
            new ReadOnlyCollection<SchoolPermission>(_allPermissions);

        public static IReadOnlyList<SchoolPermission> Root { get; } = 
            new ReadOnlyCollection<SchoolPermission>(_allPermissions.Where(p => p.IsRoot).ToArray());

        public static IReadOnlyList<SchoolPermission> Admin { get; } =
            new ReadOnlyCollection<SchoolPermission>(_allPermissions.Where(p => !p.IsRoot).ToArray());

        public static IReadOnlyList<SchoolPermission> Basic { get; } =
            new ReadOnlyCollection<SchoolPermission>(_allPermissions.Where(p => p.IsBasic).ToArray());
    }
}
