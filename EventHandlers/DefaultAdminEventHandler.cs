using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models.Pages.Admin;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration")]
    public class DefaultAdminEventHandler : IAssociativyAdminEventHandler
    {
        public void OnPageInitializing(IContent page)
        {
            if (IsPage(page, "ManageGraph"))
            {
                page.ContentItem.Weld(new AssociatvyManageGraphPart()); 
            }
            else if (IsPage(page, "Index"))
            {
                page.ContentItem.Weld(new AssociativyIndexPart()); 
            }
        }

        public void OnPageInitialized(IContent page)
        {
        }

        public void OnPageBuilt(IContent page)
        {
        }

        private bool IsPage(IContent page, string pageName)
        {
            return page.ContentItem.ContentType.EndsWith(pageName);
        }

        public void OnAuthorization(PageAutorizationContext authorizationContext)
        {
        }
    }
}