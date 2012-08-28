using Associativy.Administration.Models.Pages.Admin;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphsAdminEventHandler : IAssociativyAdminEventHandler
    {
        public void OnPageInitializing(IContent page)
        {
            if (page.IsPage("ManageGraph"))
            {
                page.ContentItem.Weld(new AssociativyManageGraphAdhocGraphPart());
            }
            else if (page.IsPage("Index"))
            {
                page.ContentItem.Weld(new AssociativyIndexAdhocGraphPart());
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