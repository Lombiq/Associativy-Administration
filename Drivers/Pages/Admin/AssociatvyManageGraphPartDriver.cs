using System.Linq;
using Associativy.Administration.Models;
using Associativy.Administration.Models.Pages.Admin;
using Associativy.Frontends.EngineDiscovery;
using Associativy.GraphDiscovery;
using Associativy.Models.Services;
using Associativy.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    public class AssociatvyMageGraphPartDriver : ContentPartDriver<AssociatvyManageGraphPart>
    {
        private readonly IGraphManager _graphManager;
        private readonly IEngineManager _engineManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IRepository<GraphSettingsRecord> _settingsRepository;

        protected override string Prefix
        {
            get { return "Associativy.Administration.AssociatvyManageGraphPart"; }
        }


        public AssociatvyMageGraphPartDriver(
            IGraphManager graphManager,
            IEngineManager engineManager,
            IWorkContextAccessor workContextAccessor,
            IRepository<GraphSettingsRecord> settingsRepository)
        {
            _graphManager = graphManager;
            _engineManager = engineManager;
            _workContextAccessor = workContextAccessor;
            _settingsRepository = settingsRepository;
        }


        protected override DriverResult Display(AssociatvyManageGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociatvyManageGraph",
            () =>
            {
                SetupSettingsLoader(part);

                var graphContext = GetGraphContext();

                part.GraphDescriptor = _graphManager.FindGraph(graphContext);
                part.FrontendEngines = _engineManager.GetEngines();
                

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
            return new GraphContext { Name = _workContextAccessor.GetContext().HttpContext.Request.QueryString["GraphName"] };
        }

        private void SetupSettingsLoader(AssociatvyManageGraphPart part)
        {
            part.SettingsRecordField.Loader(() =>
            {
                var graphName = GetGraphContext().Name;
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