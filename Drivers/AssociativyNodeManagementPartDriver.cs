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
using Piedone.HelpfulLibraries.Serialization;

namespace Associativy.Administration.Drivers
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociativyNodeManagementPartDriver : ContentPartDriver<AssociativyNodeManagementPart>
    {
        private readonly IGraphManager _graphManager;
        private readonly IAssociativyServices _associativyServices;
        private readonly ISimpleSerializer _simpleSerializer;

        protected override string Prefix
        {
            get { return "Associativy.AssociativyNodeManagementPart"; }
        }

        public AssociativyNodeManagementPartDriver(
            IGraphManager graphManager,
            IAssociativyServices associativyServices,
            ISimpleSerializer simpleSerializer)
        {
            _graphManager = graphManager;
            _associativyServices = associativyServices;
            _simpleSerializer = simpleSerializer;
        }

        //protected override DriverResult Display(AssociativyNodeManagementPart part, string displayType, dynamic shapeHelper)
        //{
        //}

        // GET
        protected override DriverResult Editor(AssociativyNodeManagementPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_AssociativyNodeManagement",
                () =>
                    {
                        FillGraphProviders(part);

                        part.GraphContextBase64s = new Dictionary<IGraphProvider, string>();
                        part.NeighbourLabels = new List<string>();

                        var context = new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } };

                        foreach (var provider in part.GraphProviders)
                        {
                            context.GraphName = provider.GraphName;
                            part.GraphContextBase64s[provider] = _simpleSerializer.Base64Serialize(context);
                            part.NeighbourLabels.Add(String.Join(", ", _associativyServices.NodeManager.GetManyContentQuery(context, provider.ConnectionManager.GetNeighbourIds(context, part.Id)).Join<AssociativyNodeLabelPartRecord>().List().Select(node => node.As<AssociativyNodeLabelPart>().Label)));
                        }

                        return shapeHelper.EditorTemplate(
                                    TemplateName: "Parts/AssociativyNodeManagement",
                                    Model: part,
                                    Prefix: Prefix);
                    });
        }

        // POST
        protected override DriverResult Editor(AssociativyNodeManagementPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);

            FillGraphProviders(part);
            var context = new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } };

            int labelsIndex = 0;
            foreach (var provider in part.GraphProviders)
            {
                // provider.ProduceContext() could be erroneous as the context with only the current content type is needed,
                // not all content types stored by the graph.
                context.GraphName = provider.GraphName;

                var newNeighbourLabels = part.NeighbourLabels[labelsIndex].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(label => label.Trim());
                var newNeighbours = _associativyServices.NodeManager.GetMany(context, newNeighbourLabels);
                
                if (newNeighbourLabels.Count() == newNeighbours.Count()) // All nodes were found
                {
                    var newNeighbourIds = newNeighbours.Select(node => node.Id).ToList();
                    var oldNeighbourIds = provider.ConnectionManager.GetNeighbourIds(context, part.Id);

                    foreach (var oldNeighbourId in oldNeighbourIds)
                    {
                        if (!newNeighbourIds.Contains(oldNeighbourId))
                        {
                            provider.ConnectionManager.Disconnect(context, part.Id, oldNeighbourId);
                        }
                        else newNeighbourIds.Remove(oldNeighbourId); // So newNeighbourIds will contain only really new node ids
                    }

                    foreach (var neighbourId in newNeighbourIds)
                    {
                        provider.ConnectionManager.Connect(context, part.Id, neighbourId);
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

        private void FillGraphProviders(AssociativyNodeManagementPart part)
        {
            var grapProviders = _graphManager.FindProviders(new GraphContext { ContentTypes = new string[] { part.ContentItem.ContentType } });
            if (grapProviders.Count() != 0)
            {
                part.GraphProviders = grapProviders;
            }
        }
    }
}