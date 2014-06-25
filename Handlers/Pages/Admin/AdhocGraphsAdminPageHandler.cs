using Associativy.Administration.Models.Pages.Admin;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Piedone.HelpfulLibraries.Contents.DynamicPages;
using Piedone.HelpfulLibraries.Contents;

namespace Associativy.Administration.Handlers
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphsAdminPageHandler : ContentHandler
    {
        protected override void Initializing(InitializingContentContext context)
        {
            var pageContext = context.PageContext();

            if (pageContext.Group != AdministrationPageConfigs.Group) return;

            if (pageContext.Page.IsPage("ManageGraph", pageContext.Group))
            {
                pageContext.Page.Weld<AssociativyManageGraphAdhocGraphPart>();
            }
            else if (pageContext.Page.IsPage("Index", pageContext.Group))
            {
                pageContext.Page.Weld<AssociativyIndexAdhocGraphPart>();
            }
        }
    }
}