using Associativy.Administration.Models.Pages.Admin;
using Associativy.Administration.Services;
using Associativy.Frontends.EngineDiscovery;
using Associativy.GraphDiscovery;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Mvc;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    public class AssociatvyMageGraphPartDriver : ContentPartDriver<AssociatvyManageGraphPart>
    {
        private readonly IHttpContextAccessor _hca;
        private readonly IEngineManager _engineManager;
        private readonly IGraphSettingsService _settingsService;

        protected override string Prefix
        {
            get { return "Associativy.Administration.AssociatvyManageGraphPart"; }
        }


        public AssociatvyMageGraphPartDriver(
            IHttpContextAccessor hca,
            IEngineManager engineManager,
            IGraphSettingsService settingsService)
        {
            _hca = hca;
            _engineManager = engineManager;
            _settingsService = settingsService;
        }


        protected override DriverResult Display(AssociatvyManageGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociatvyManageGraph",
            () =>
            {
                SetupSettingsLoader(part);

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

            if (part.GraphSettings.InitialZoomLevel > part.GraphSettings.ZoomLevelCount) part.GraphSettings.InitialZoomLevel = part.GraphSettings.ZoomLevelCount - 1;

            return Display(part, "Detail", shapeHelper);
        }

        private GraphContext GetGraphContext()
        {
            return new GraphContext { Name = _hca.Current().Request.QueryString["GraphName"] };
        }

        private void SetupSettingsLoader(AssociatvyManageGraphPart part)
        {
            part.SettingsField.Loader(() =>_settingsService.GetSettings(GetGraphContext().Name));
        }
    }
}