using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models;
using Associativy.Services;
using Associativy.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Associativy.GraphDiscovery;

namespace Associativy.Administration.Drivers
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociativyNodeManagementPartDriver : ContentPartDriver<AssociativyNodeManagementPart>
    {
        private readonly IGraphManager _graphManager;
        private readonly IAssociativyServices _associativyServices;

        protected override string Prefix
        {
            get { return "Associativy.AssociativyNodeManagementPart"; }
        }

        public AssociativyNodeManagementPartDriver(
            IGraphManager graphManager,
            IAssociativyServices associativyServices)
        {
            _graphManager = graphManager;
            _associativyServices = associativyServices;
        }

        //protected override DriverResult Display(AssociativyNodeManagementPart part, string displayType, dynamic shapeHelper)
        //{
        //}

        // GET
        protected override DriverResult Editor(AssociativyNodeManagementPart part, dynamic shapeHelper)
        {
            FillGraphDescriptors(part);

            part.NeighbourLabels = new Dictionary<string, string>();
            var context = new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } };
            foreach (var descriptor in part.GraphDescriptors)
            {
                context.GraphName = descriptor.GraphName;
                part.NeighbourLabels[descriptor.GraphName] = String.Join(", ", _associativyServices.NodeManager.GetManyContentQuery(context, descriptor.ConnectionManager.GetNeighbourIds(context, part.Id)).Join<AssociativyNodeLabelPartRecord>().List().Select(node => node.As<AssociativyNodeLabelPart>().Label));
            }

            return ContentShape("Parts_AssociativyNodeManagement",
                () => shapeHelper.EditorTemplate(
                        TemplateName: "Parts/AssociativyNodeManagement",
                        Model: part,
                        Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(AssociativyNodeManagementPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            FillGraphDescriptors(part);
            var context = new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } };

            foreach (var descriptor in part.GraphDescriptors)
            {
                // descriptor.ProduceContext() could be erroneous as the context with only the current content type is needed,
                // not all content types stored by the graph.
                context.GraphName = descriptor.GraphName;

                var newNeighbourLabels = part.NeighbourLabels[descriptor.GraphName].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(label => label.Trim());
                var newNeighbours = _associativyServices.NodeManager.GetMany(context, newNeighbourLabels);
                
                if (newNeighbourLabels.Count() == newNeighbours.Count()) // All nodes were found
                {
                    var newNeighbourIds = newNeighbours.Select(node => node.Id).ToList();
                    var oldNeighbourIds = descriptor.ConnectionManager.GetNeighbourIds(context, part.Id);

                    foreach (var oldNeighbourId in oldNeighbourIds)
                    {
                        if (!newNeighbourIds.Contains(oldNeighbourId))
                        {
                            descriptor.ConnectionManager.Disconnect(context, part.Id, oldNeighbourId);
                        }
                        else newNeighbourIds.Remove(oldNeighbourId); // So newNeighbourIds will contain only really new node ids
                    }

                    foreach (var neighbourId in newNeighbourIds)
                    {
                        descriptor.ConnectionManager.Connect(context, part.Id, neighbourId);
                    }
                }
            }

            // This is so the code in the above Editor method doesn't get run unneeded when posting the form.
            // However this will cause ContentManager's UpdateEditorShape() to return null. This is no problem here, revise if
            // necessary.
            return null;
            //return Editor(part, shapeHelper);
        }

        private void FillGraphDescriptors(AssociativyNodeManagementPart part)
        {
            var graphDescriptors = _graphManager.FindDescriptors(new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } });
            if (graphDescriptors.Count() != 0)
            {
                part.GraphDescriptors = graphDescriptors;
            }
        }
    }
}