using System.Collections.Generic;
using System.Linq;
using Smartstore.Core.Identity;
using Smartstore.Core.Security;

namespace Smartstore.Iyzico;

/// <summary>
/// All permissions provided by this module. Recommended to use singular for names, <see cref="Permissions"/>.
/// "iyzico" is the root permission (by convention, doesn't contain any dot). Localization key is "Modules.Permissions.DisplayName.Iyzico".
/// "iyzico.read" and "iyzico.update" do not need localization because they are contained in core, <see cref="PermissionService._displayNameResourceKeys"/>.
/// </summary>
internal static class IyzicoPermissions
{
    public const string Self = "iyzico";
}

internal class DevToolsPermissionProvider : IPermissionProvider
{
    public IEnumerable<PermissionRecord> GetPermissions()
    {
        // Get all permissions from above static class.
        var permissionSystemNames = PermissionHelper.GetPermissions(typeof(IyzicoPermissions));
        var permissions = permissionSystemNames.Select(x => new PermissionRecord { SystemName = x });

        return permissions;
    }

    public IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
    {
        // Allow root permission for admin by default.
        return
        [
            new DefaultPermissionRecord
            {
                CustomerRoleSystemName = SystemCustomerRoleNames.Administrators,
                PermissionRecords =
                [
                    new PermissionRecord { SystemName = IyzicoPermissions.Self }
                ]
            }
        ];
    }
}
