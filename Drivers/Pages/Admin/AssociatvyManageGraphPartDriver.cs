using System.Collections.Generic;
using Associativy.Administration.Models.Pages.Admin;
using Associativy.Administration.Services;
using Associativy.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData.Models;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    public class AssociatvyMageGraphPartDriver : ContentPartDriver<AssociatvyManageGraphPart>
    {
        private readonly IGraphSettingsService _settingsService;
        private readonly IContentManager _contentManager;

        protected override string Prefix
        {
            get { return "Associativy.Administration.AssociatvyManageGraphPart"; }
        }


        public AssociatvyMageGraphPartDriver(
            IGraphSettingsService settingsService,
            IContentManager contentManager)
        {
            _settingsService = settingsService;
            _contentManager = contentManager;
        }


        protected override DriverResult Display(AssociatvyManageGraphPart part, string displayType, dynamic shapeHelper)
        {
            return Editor(part, shapeHelper);
        }

        protected override DriverResult Editor(AssociatvyManageGraphPart part, dynamic shapeHelper)
        {
            SetupLazyLoaders(part);

            return Combined(
                ContentShape("Pages_AssociatvyManageGraph_GraphInfo",
                    () => shapeHelper.DisplayTemplate(
                                    TemplateName: "Pages/Admin/ManageGraph.GraphInfo",
                                    Model: part,
                                    Prefix: Prefix)),
                ContentShape("Pages_AssociatvyManageGraph_Settings",
                    () => shapeHelper.Pages_AssociatvyManageGraph_Settings()),
                ContentShape("Pages_AssociatvyManageGraph_Settings_ImplicitlyCreatableContentType",
                    () => shapeHelper.DisplayTemplate(
                                    TemplateName: "Pages/Admin/ManageGraph.Settings.ImplicitlyCreatableContentType",
                                    Model: part,
                                    Prefix: Prefix)),
                ContentShape("Pages_AssociatvyManageGraph_ImportExport",
                    () => shapeHelper.DisplayTemplate(
                                    TemplateName: "Pages/Admin/ManageGraph.ImportExport",
                                    Model: part,
                                    Prefix: Prefix))
                );
        }

        protected override DriverResult Editor(AssociatvyManageGraphPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            SetupLazyLoaders(part);

            updater.TryUpdateModel(part, Prefix, null, null);

            if (part.GraphSettings.InitialZoomLevel > part.GraphSettings.ZoomLevelCount) part.GraphSettings.InitialZoomLevel = part.GraphSettings.ZoomLevelCount - 1;

            return Editor(part, shapeHelper);
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