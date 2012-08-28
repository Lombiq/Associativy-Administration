using System.Linq;
using Associativy.Administration.Models;
using Associativy.Administration.Models.Pages.Admin;
using Associativy.Frontends.EngineDiscovery;
using Associativy.GraphDiscovery;
using Associativy.Models.Mind;
using Associativy.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociatvyMageGraphPartDriver : ContentPartDriver<AssociatvyManageGraphPart>
    {
        private readonly IAssociativyServices _associativyServices;
        private readonly IEngineManager _engineManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IRepository<GraphSettingsRecord> _settingsRepository;

        protected override string Prefix
        {
            get { return "Associativy.Administration.AssociatvyManageGraphPart"; }
        }

        public AssociatvyMageGraphPartDriver(
            IAssociativyServices associativyServices,
            IEngineManager engineManager,
            IWorkContextAccessor workContextAccessor,
            IRepository<GraphSettingsRecord> settingsRepository)
        {
            _associativyServices = associativyServices;
            _engineManager = engineManager;
            _workContextAccessor = workContextAccessor;
            _settingsRepository = settingsRepository;
        }

        protected override DriverResult Display(AssociatvyManageGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociatvyManageGraph",
            () =>
            {
                var graphContext = GetGraphContext();
                var graph = _associativyServices.Mind.GetAllAssociations(graphContext, new MindSettings { ZoomLevelCount = 1 });

                part.GraphDescriptor = _associativyServices.GraphManager.FindGraph(graphContext);
                part.NodeCount = graph.VertexCount;
                part.ConnectionCount = graph.EdgeCount;
                part.FrontendEngines = _engineManager.GetEngines();

                SetupSettingsLoader(part);

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/ManageGraph",
                            Model: part,
                            Prefix: Prefix);
            });
        }

        protected override DriverResult Editor(AssociatvyManageGraphPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            SetupSettingsLoader(part);

            updater.TryUpdateModel(part, Prefix, null, null);

            if (part.InitialZoomLevel > part.ZoomLevelCount) part.InitialZoomLevel = part.ZoomLevelCount - 1;

            return Display(part, "Detail", shapeHelper);
        }

        private GraphContext GetGraphContext()
        {
            return new GraphContext { GraphName = _workContextAccessor.GetContext().HttpContext.Request.QueryString["GraphName"] };
        }

        private void SetupSettingsLoader(AssociatvyManageGraphPart part)
        {
            part.SettingsRecordField.Loader(() =>
            {
                var graphName = GetGraphContext().GraphName;
                var settings = _settingsRepository.Fetch(record => record.GraphName == graphName).SingleOrDefault();
                if (settings == null)
                {
                    settings = new GraphSettingsRecord
                    {
                        GraphName = graphName
                    };
                    _settingsRepository.Create(settings);
                }

                return settings;
            });
        }
    }
}