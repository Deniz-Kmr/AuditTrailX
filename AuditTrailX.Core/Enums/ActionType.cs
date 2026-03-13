namespace AuditTrailX.Core.Enums;

public enum ActionType
{
    Created = 1,
    Updated = 2,
    Deleted = 3,
    Login = 4,
    Logout = 5,
    PermissionChanged = 6,
    SettingChanged = 7,
    PasswordChanged = 8,
    RoleAssigned = 9,
    PaymentStatusUpdated = 10
}