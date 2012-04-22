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
    }
}