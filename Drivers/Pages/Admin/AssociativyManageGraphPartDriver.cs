using System.Collections.Generic;
using Associativy.Administration.Models.Pages.Admin;
using Associativy.Administration.Services;
using Associativy.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData.Models;
using Piedone.HelpfulLibraries.KeyValueStore;
using Associativy.Administration;
using Associativy.Administration.Models;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    public class AssociativyManageGraphPartDriver : ContentPartDriver<AssociativyManageGraphPart>
    {
        private readonly IGraphSettingsService _settingsService;
        private readonly IContentManager _contentManager;

        protected override string Prefix
        {
            get { return "Associativy.Administration.AssociativyManageGraphPart"; }
        }


        public AssociativyManageGraphPartDriver(
            IGraphSettingsService settingsService,
            IContentManager contentManager)
        {
            _settingsService = settingsService;
            _contentManager = contentManager;
        }


        protected override DriverResult Display(AssociativyManageGraphPart part, string displayType, dynamic shapeHelper)
        {
            return Editor(part, shapeHelper);
        }

        protected override DriverResult Editor(AssociativyManageGraphPart part, dynamic shapeHelper)
        {
            SetupLazyLoaders(part);

            return Combined(
                ContentShape("Pages_AssociativyManageGraph_GraphInfo",
                    () => shapeHelper.DisplayTemplate(
                                    TemplateName: "Pages/Admin/ManageGraph.GraphInfo",
                                    Model: part,
                                    Prefix: Prefix)),
                ContentShape("Pages_AssociativyManageGraph_AdminSettings",
                    () => shapeHelper.DisplayTemplate(
                                    TemplateName: "Pages/Admin/ManageGraph.AdminSettings",
                                    Model: part,
                                    Prefix: Prefix)),
                ContentShape("Pages_AssociativyManageGraph_ImportExport",
                    () => shapeHelper.DisplayTemplate(
                                    TemplateName: "Pages/Admin/ManageGraph.ImportExport",
                                    Model: part,
                                    Prefix: Prefix))
                );
        }

        protected override DriverResult Editor(AssociativyManageGraphPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            SetupLazyLoaders(part);

            updater.TryUpdateModel(part, Prefix, null, null);
            _settingsService.Set(part.GraphDescriptor.Name, part.GraphSettings);

            return Editor(part, shapeHelper);
        }

        private void SetupLazyLoaders(AssociativyManageGraphPart part)
        {
            part.SettingsField.Loader(() => _settingsService.GetNotNull<GraphSettings>(part.GraphDescriptor.Name));
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