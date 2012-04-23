using Associativy.Administration.Models.Admin;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

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
        }

        public void OnPageInitialized(IContent page)
        {
        }

        public void OnPageBuilt(IContent page)
        {
        }
    }
}