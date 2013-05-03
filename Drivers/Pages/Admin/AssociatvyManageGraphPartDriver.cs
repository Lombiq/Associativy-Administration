using System.Collections.Generic;
using Associativy.Administration.Models.Pages.Admin;
using Associativy.Administration.Services;
using Associativy.Frontends.EngineDiscovery;
using Associativy.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData.Models;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    public class AssociatvyMageGraphPartDriver : ContentPartDriver<AssociatvyManageGraphPart>
    {
        private readonly IEngineManager _engineManager;
        private readonly IGraphSettingsService _settingsService;
        private readonly IContentManager _contentManager;

        protected override string Prefix
        {
            get { return "Associativy.Administration.AssociatvyManageGraphPart"; }
        }


        public AssociatvyMageGraphPartDriver(
            IEngineManager engineManager,
            IGraphSettingsService settingsService,
            IContentManager contentManager)
        {
            _engineManager = engineManager;
            _settingsService = settingsService;
            _contentManager = contentManager;
        }


        protected override DriverResult Display(AssociatvyManageGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociatvyManageGraph",
            () =>
            {
                SetupLazyLoaders(part);

                part.FrontendEngines = _engineManager.GetEngines();

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/ManageGraph",
                            Model: part,
                            Prefix: Prefix);
            });
        }

        protected override DriverResult Editor(AssociatvyManageGraphPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            SetupLazyLoaders(part);

            updater.TryUpdateModel(part, Prefix, null, null);

            if (part.GraphSettings.InitialZoomLevel > part.GraphSettings.ZoomLevelCount) part.GraphSettings.InitialZoomLevel = part.GraphSettings.ZoomLevelCount - 1;

            return Display(part, "Detail", shapeHelper);
        }

        private void SetupLazyLoaders(AssociatvyManageGraphPart part)
        {
            part.SettingsField.Loader(() => _settingsService.GetSettings(part.GraphDescriptor.Name));
            part.ImplicitlyCreatableContentTypesField.Loader(() =>
                {
                    var implicitlyCreatableTypes = new List<ContentTypeDefinition>();

                    foreach (var type in part.GraphDescriptor.ContentTypes)
                    {
                        // This seems to be the only way to check the existence of an aspect
                        var item = _contentManager.New(type);
                        if (item.As<IImplicitlyCreatableAssociativyNodeAspect>() != null)
                        {
                            implicitlyCreatableTypes.Add(item.TypeDefinition);
                        }
                    }

                    return implicitlyCreatableTypes;
                });
        }
    }
}