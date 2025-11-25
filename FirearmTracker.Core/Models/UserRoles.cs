namespace FirearmTracker.Core.Models
{
    public static class UserRoles
    {
        public const string Owner = "Owner";
        public const string Administrator = "Administrator";
        public const string PowerUser = "PowerUser";
        public const string ReadOnly = "ReadOnly";

        public static readonly string[] AllRoles = { Owner, Administrator, PowerUser, ReadOnly };

        public static bool IsValidRole(string role)
        {
            return AllRoles.Contains(role);
        }

        public static string GetRoleDisplayName(string role)
        {
            return role switch
            {
                Owner => "Owner",
                Administrator => "Administrator",
                PowerUser => "Power User",
                ReadOnly => "Read-only",
                _ => role
            };
        }

        public static bool CanManageUsers(string role)
        {
            return role == Owner || role == Administrator;
        }

        public static bool CanEdit(string role)
        {
            return role != ReadOnly;
        }

        public static bool CanDelete(string role)
        {
            return role != ReadOnly;
        }

        public static bool IsOwner(string role)
        {
            return role == Owner;
        }
    }
}