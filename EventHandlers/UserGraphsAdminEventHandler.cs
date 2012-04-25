using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models.Pages.Admin;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class UserGraphsAdminEventHandler : IAdminEventHandler
    {
        public void OnPageInitializing(IContent page)
        {
            if (page.IsPage("ManageGraph"))
            {
                page.ContentItem.Weld(new AssociativyManageGraphUserGraphPart());
            }
            else if (page.IsPage("Index"))
            {
                page.ContentItem.Weld(new AssociativyIndexUserGraphPart());
            }
        }

        public void OnPageInitialized(IContent page)
        {
        }

        public void OnPageBuilt(IContent page)
        {
        }
    }
}