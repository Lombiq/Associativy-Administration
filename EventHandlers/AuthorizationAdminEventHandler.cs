using Associativy.Administration.Models.Pages.Admin;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class AuthorizationAdminEventHandler : IPageEventHandler
    {
        public void OnPageInitializing(PageContext pageContext)
        {
            if (pageContext.Group != AdministrationPageConfigs.Group) return;

            var page = pageContext.Page;
            if (page.IsPage("ManageGraph", pageContext.Group))
            {
                page.ContentItem.Weld(new AssociativyManageGraphAuthorizationPart());
            }
        }

        public void OnPageInitialized(PageContext pageContext)
        {
        }

        public void OnPageBuilt(PageContext pageContext)
        {
        }

        public void OnAuthorization(PageAutorizationContext authorizationContext)
        {
        }
    }
}