using System;
using System.Collections.Generic;
using System.Linq;
using Associativy.Administration.Models;
using Associativy.GraphDiscovery;
using Associativy.Models;
using Associativy.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

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
            return ContentShape("Parts_AssociativyNodeManagement_Edit",
                () =>
                    {
                        FillGraphDescriptors(part);

                        part.GraphContexts = new Dictionary<GraphDescriptor, IGraphContext>();
                        part.NeighbourLabels = new List<string>();

                        var context = new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } };

                        foreach (var provider in part.GraphDescriptors)
                        {
                            context.GraphName = provider.GraphName;
                            part.GraphContexts[provider] = context;
                            part.NeighbourLabels.Add(String.Join(", ", _associativyServices.NodeManager.GetManyQuery(context, provider.PathServices.ConnectionManager.GetNeighbourIds(context, part.ContentItem.Id)).List().Select(node => node.As<IAssociativyNodeLabelAspect>().Label)));
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
            foreach (var provider in part.GraphDescriptors)
            {
                // provider.ProduceContext() could be erroneous as the context with only the current content type is needed,
                // not all content types stored by the graph.
                context.GraphName = provider.GraphName;

                var newNeighbourLabels = part.NeighbourLabels[labelsIndex].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(label => label.Trim());
                var newNeighbours = _associativyServices.NodeManager.GetManySimilarNodesQuery(context, newNeighbourLabels).List();
                
                if (newNeighbourLabels.Count() == newNeighbours.Count()) // All nodes were found
                {
                    var newNeighbourIds = new HashSet<int>(newNeighbours.Select(node => node.Id));
                    var oldNeighbourIds = provider.PathServices.ConnectionManager.GetNeighbourIds(context, part.ContentItem.Id);

                    foreach (var oldNeighbourId in oldNeighbourIds)
                    {
                        if (!newNeighbourIds.Contains(oldNeighbourId))
                        {
                            provider.PathServices.ConnectionManager.Disconnect(context, part.ContentItem.Id, oldNeighbourId);
                        }
                        else newNeighbourIds.Remove(oldNeighbourId); // So newNeighbourIds will contain only really new node ids
                    }

                    foreach (var neighbourId in newNeighbourIds)
                    {
                        provider.PathServices.ConnectionManager.Connect(context, part.ContentItem.Id, neighbourId);
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