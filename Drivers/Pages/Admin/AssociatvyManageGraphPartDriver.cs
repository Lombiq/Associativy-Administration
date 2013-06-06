using System.Collections.Generic;
using Associativy.Administration.Models.Pages.Admin;
using Associativy.Administration.Services;
using Associativy.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData.Models;
using Piedone.HelpfulLibraries.KeyValueStore;
using Associativy.Administration;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    public class AssociatvyMageGraphPartDriver : ContentPartDriver<AssociatvyManageGraphPart>
    {
        private readonly IKeyValueStore _keyValueStore;
        private readonly IContentManager _contentManager;

        protected override string Prefix
        {
            get { return "Associativy.Administration.AssociatvyManageGraphPart"; }
        }


        public AssociatvyMageGraphPartDriver(
            IKeyValueStore keyValueStore,
            IContentManager contentManager)
        {
            _keyValueStore = keyValueStore;
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
                ContentShape("Pages_AssociatvyManageGraph_AdminSettings",
                    () => shapeHelper.DisplayTemplate(
                                    TemplateName: "Pages/Admin/ManageGraph.AdminSettings",
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
            _keyValueStore.SetGraphSettings(part.GraphDescriptor.Name, part.GraphSettings);

            return Editor(part, shapeHelper);
        }

        private void SetupLazyLoaders(AssociatvyManageGraphPart part)
        {
            part.SettingsField.Loader(() => _keyValueStore.GetGraphSettings(part.GraphDescriptor.Name));
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