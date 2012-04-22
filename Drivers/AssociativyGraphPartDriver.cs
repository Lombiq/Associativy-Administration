using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Associativy.Administration.Models;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;

namespace Associativy.Administration.Drivers
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyGraphPartDriver : ContentPartDriver<AssociativyGraphPart>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        protected override string Prefix
        {
            get { return "Associativy.Administration.UserGraph.AssociativyGraphPartDriver"; }
        }

        public AssociativyGraphPartDriver(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        protected override DriverResult Display(AssociativyGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_AssociativyGraph",
                () => shapeHelper.Parts_AssociativyGraph());
        }

        // GET
        protected override DriverResult Editor(AssociativyGraphPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_AssociativyGraph_Edit",
                () => shapeHelper.EditorTemplate(
                        TemplateName: "Parts.AssociativyGraph",
                        Model: part,
                        Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(AssociativyGraphPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            return Editor(part, shapeHelper);
        }
    }
}