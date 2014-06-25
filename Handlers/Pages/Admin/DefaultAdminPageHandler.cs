using Associativy.Administration.Models.Pages.Admin;
using Associativy.GraphDiscovery;
using Orchard.ContentManagement.Handlers;
using Orchard.Mvc;
using Piedone.HelpfulLibraries.Contents.DynamicPages;
using Piedone.HelpfulLibraries.Contents;

namespace Associativy.Administration.Handlers
{
    public class DefaultAdminPageHandler : ContentHandler
    {
        private readonly IHttpContextAccessor _hca;
        private readonly IGraphManager _graphManager;


        public DefaultAdminPageHandler(IHttpContextAccessor hca, IGraphManager graphManager)
        {
            _hca = hca;
            _graphManager = graphManager;
        }


        protected override void Initializing(InitializingContentContext context)
        {
            var pageContext = context.PageContext();

            if (pageContext.Group != AdministrationPageConfigs.Group) return;

            if (pageContext.Page.IsPage("ManageGraph", pageContext.Group))
            {
                pageContext.Page.Weld<AssociativyManageGraphPart>(part => part.GraphDescriptor = _graphManager.FindGraphByName(_hca.Current().Request.QueryString["GraphName"]));
            }
            else if (pageContext.Page.IsPage("Index", pageContext.Group))
            {
                pageContext.Page.Weld<AssociativyIndexPart>();
            }
        }
    }
}