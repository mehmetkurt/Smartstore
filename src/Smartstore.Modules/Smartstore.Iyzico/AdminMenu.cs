using Smartstore.Collections;
using Smartstore.Core.Content.Menus;
using Smartstore.Web.Rendering.Builders;

namespace Smartstore.Iyzico
{
    public class AdminMenu : AdminMenuProvider
    {
        protected override void BuildMenuCore(TreeNode<MenuItem> modulesNode)
        {
            var menuItem = new MenuItem().ToBuilder()
                .Text("Iyzico")
                .ResKey("Plugins.FriendlyName.SmartStore.Iyzico")
                .Icon("credit-card", "bi")
                .PermissionNames(IyzicoPermissions.Self)
                .Action("Configure", "IyzicoAdmin", new { area = "Admin" })
                .AsItem();

            modulesNode.Append(menuItem);
        }
    }
}
