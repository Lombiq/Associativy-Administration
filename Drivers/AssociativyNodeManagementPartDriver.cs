using System;
using System.Collections.Generic;
using System.Linq;
using Associativy.Administration.Models;
using Associativy.GraphDiscovery;
using Associativy.Models;
using Associativy.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Associativy.Administration.Drivers
{
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
            return ContentShape("Parts_AssociativyNodeManagement_Edit",
                () =>
                    {
                        FillGraphDescriptors(part);

                        part.GraphContexts = new Dictionary<IGraphDescriptor, IGraphContext>();
                        part.NeighbourLabels = new List<string>();

                        var context = new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } };

                        foreach (var descriptor in part.GraphDescriptors)
                        {
                            context.Name = descriptor.Name;
                            part.GraphContexts[descriptor] = context;
                            part.NeighbourLabels.Add(String.Join(", ", descriptor.Services.NodeManager.GetManyQuery(descriptor.Services.ConnectionManager.GetNeighbourIds(part.ContentItem.Id)).List().Select(node => node.As<IAssociativyNodeLabelAspect>().Label)));
                        }

                        return shapeHelper.EditorTemplate(
                                    TemplateName: "Parts.AssociativyNodeManagement",
                                    Model: part,
                                    Prefix: Prefix);
                    });
        }

        // POST
        protected override DriverResult Editor(AssociativyNodeManagementPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            FillGraphDescriptors(part);
            var context = new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } };

            int labelsIndex = 0;
            foreach (var descriptor in part.GraphDescriptors)
            {
                // provider.ProduceContext() could be erroneous as the context with only the current content type is needed,
                // not all content types stored by the graph.
                context.Name = descriptor.Name;

                var newNeighbourLabels = part.NeighbourLabels[labelsIndex].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(label => label.Trim());
                var newNeighbours = descriptor.Services.NodeManager.GetManyByLabelQuery(newNeighbourLabels).List();
                
                if (newNeighbourLabels.Count() == newNeighbours.Count()) // All nodes were found
                {
                    var newNeighbourIds = new HashSet<int>(newNeighbours.Select(node => node.Id));
                    var oldNeighbourIds = descriptor.Services.ConnectionManager.GetNeighbourIds(part.ContentItem.Id);

                    foreach (var oldNeighbourId in oldNeighbourIds)
                    {
                        if (!newNeighbourIds.Contains(oldNeighbourId))
                        {
                            descriptor.Services.ConnectionManager.Disconnect(part.ContentItem.Id, oldNeighbourId);
                        }
                        else newNeighbourIds.Remove(oldNeighbourId); // So newNeighbourIds will contain only really new node ids
                    }

                    foreach (var neighbourId in newNeighbourIds)
                    {
                        descriptor.Services.ConnectionManager.Connect(part.ContentItem.Id, neighbourId);
                    }
                }

                labelsIndex++;
            }

            // This is so the code in the above Editor method doesn't get run unneeded when posting the form.
            // However this will cause ContentManager's UpdateEditorShape() to return null. This is no problem here, revise if
            // necessary.
            return null;
            //return Editor(part, shapeHelper);
        }

        private void FillGraphDescriptors(AssociativyNodeManagementPart part)
        {
            part.GraphDescriptors = _graphManager.FindDistinctGraphs(new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } });
        }
    }
}