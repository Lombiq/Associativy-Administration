using System;
using System.Linq;
using Associativy.Administration.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement.Handlers;

namespace Associativy.Administration.Drivers
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyGraphPartDriver : ContentPartDriver<AssociativyGraphPart>
    {
        protected override string Prefix
        {
            get { return "Associativy.Administration.UserGraph.AssociativyGraphPartDriver"; }
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
            var name = part.GraphName;
            updater.TryUpdateModel(part, Prefix, null, null);
            if (!String.IsNullOrWhiteSpace(name) && part.GraphName != name) part.GraphName = name; // This is to prevent modification of the name

            part.ContainedContentTypes = part.AllContentTypes.Where(type => type.IsContained).Select(type => type.Name).ToList();

            return Editor(part, shapeHelper);
        }

        protected override void Exporting(AssociativyGraphPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("GraphName", part.GraphName);
            element.SetAttributeValue("DisplayGraphName", part.DisplayGraphName);
            element.SetAttributeValue("ContainedContentTypes", part.Record.ContainedContentTypes);
        }

        protected override void Importing(AssociativyGraphPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            context.ImportAttribute(partName, "GraphName", value => part.GraphName = value);
            context.ImportAttribute(partName, "DisplayGraphName", value => part.DisplayGraphName = value);
            context.ImportAttribute(partName, "ContainedContentTypes", value => part.Record.ContainedContentTypes = value);
        }
    }
}