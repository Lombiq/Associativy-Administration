using Associativy.Administration.Models.Pages.Admin;
using Associativy.GraphDiscovery;
using Orchard.Mvc;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    public class DefaultAdminEventHandler : IPageEventHandler
    {
        private readonly IHttpContextAccessor _hca;
        private readonly IGraphManager _graphManager;


        public DefaultAdminEventHandler(IHttpContextAccessor hca, IGraphManager graphManager)
        {
            _hca = hca;
            _graphManager = graphManager;
        }


        public void OnPageInitializing(PageContext pageContext)
        {
            if (pageContext.Group != AdministrationPageConfigs.Group) return;

            var page = pageContext.Page;
            if (page.IsPage("ManageGraph", pageContext.Group))
            {
                page.ContentItem.Weld(new AssociativyManageGraphPart { GraphDescriptor = _graphManager.FindGraph(_hca.Current().Request.QueryString["GraphName"]) });
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