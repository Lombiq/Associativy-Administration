using Associativy.GraphDiscovery;
using Associativy.Models.Mind;
using Associativy.Services;
using Orchard;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models.Pages.Admin;
using Associativy.Frontends.EngineDiscovery;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociatvyMageGraphPartDriver : ContentPartDriver<AssociatvyManageGraphPart>
    {
        private readonly IAssociativyServices _associativyServices;
        private readonly IEngineManager _engineManager;
        private readonly IWorkContextAccessor _workContextAccessor;

        public AssociatvyMageGraphPartDriver(
            IAssociativyServices associativyServices,
            IEngineManager engineManager,
            IWorkContextAccessor workContextAccessor)
        {
            _associativyServices = associativyServices;
            _engineManager = engineManager;
            _workContextAccessor = workContextAccessor;
        }

        protected override DriverResult Display(AssociatvyManageGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociatvyManageGraph",
            () =>
            {
                var graphContext = new GraphContext { GraphName = _workContextAccessor.GetContext().HttpContext.Request.QueryString["GraphName"] };
                var graph = _associativyServices.Mind.GetAllAssociations(graphContext, new MindSettings { ZoomLevelCount = 1 });

                part.GraphDescriptor = _associativyServices.GraphManager.FindGraph(graphContext);
                part.NodeCount = graph.VertexCount;
                part.ConnectionCount = graph.EdgeCount;
                part.FrontendEngines = _engineManager.GetEngines();

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/ManageGraph",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}