using Associativy.Administration.Models.Pages.Admin;
using Orchard.Environment.Extensions;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    public class DefaultAdminEventHandler : IPageEventHandler
    {
        public void OnPageInitializing(PageContext pageContext)
        {
            if (pageContext.Group != AdministrationPageConfigs.Group) return;

            var page = pageContext.Page;
            if (page.IsPage("ManageGraph", pageContext.Group))
            {
                page.ContentItem.Weld(new AssociatvyManageGraphPart()); 
            }
            else if (page.IsPage("Index", pageContext.Group))
            {
                page.ContentItem.Weld(new AssociativyIndexPart()); 
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