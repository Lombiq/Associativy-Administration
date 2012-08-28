using Associativy.Administration.Models.Pages.Admin;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration")]
    public class DefaultAdminEventHandler : IAssociativyAdminEventHandler
    {
        public void OnPageInitializing(IContent page)
        {
            if (page.IsPage("ManageGraph"))
            {
                page.ContentItem.Weld(new AssociatvyManageGraphPart()); 
            }
            else if (page.IsPage("Index"))
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

        public void OnAuthorization(PageAutorizationContext authorizationContext)
        {
        }
    }
}