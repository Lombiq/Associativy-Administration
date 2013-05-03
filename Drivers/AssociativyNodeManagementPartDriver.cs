﻿using System;
using System.Collections.Generic;
using System.Linq;
using Associativy.Administration.Models;
using Associativy.Administration.Services;
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
        private readonly IGraphSettingsService _settingsService;

        protected override string Prefix
        {
            get { return "Associativy.AssociativyNodeManagementPart"; }
        }


        public AssociativyNodeManagementPartDriver(
            IGraphManager graphManager,
            IAssociativyServices associativyServices,
            IGraphSettingsService settingsService)
        {
            _graphManager = graphManager;
            _associativyServices = associativyServices;
            _settingsService = settingsService;
        }


        // GET
        protected override DriverResult Editor(AssociativyNodeManagementPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_AssociativyNodeManagement_Edit",
                () =>
                {
                    FillGraphDescriptors(part);

                    part.NeighbourValues = new List<NeighbourValues>();

                    foreach (var descriptor in part.GraphDescriptors)
                    {
                        var services = descriptor.Services;
                        var values = new NeighbourValues { NeighbourCount = services.ConnectionManager.GetNeighbourCount(part) };

                        if (values.NeighbourCount <= _settingsService.GetSettings(descriptor.Name).MaxConnectionCount)
                        {
                            values.ShowLabels = true;
                            values.Labels = string.Join(", ", services.NodeManager.GetQuery().ForContentItems(services.ConnectionManager.GetNeighbourIds(part.ContentItem.Id)).List().Select(node => node.As<IAssociativyNodeLabelAspect>().Label));
                        }

                        part.NeighbourValues.Add(values);
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

            int labelsIndex = 0;
            foreach (var descriptor in part.GraphDescriptors)
            {
                var connectionManager = descriptor.Services.ConnectionManager;
                var nodeManager = descriptor.Services.NodeManager;
                var neighbourValues = part.NeighbourValues[labelsIndex];
                neighbourValues.NeighbourCount = connectionManager.GetNeighbourCount(part);

                if (neighbourValues.NeighbourCount <= _settingsService.GetSettings(descriptor.Name).MaxConnectionCount)
                {
                    neighbourValues.ShowLabels = true;

                    if (string.IsNullOrEmpty(neighbourValues.Labels))
                    {
                        connectionManager.DeleteFromNode(part);
                    }

                    ProcessLabels(neighbourValues.Labels, nodeManager, newNeighbours =>
                        {
                            var newNeighbourIds = new HashSet<int>(newNeighbours.Select(node => node.Id));
                            var oldNeighbourIds = connectionManager.GetNeighbourIds(part.ContentItem.Id);

                            foreach (var oldNeighbourId in oldNeighbourIds)
                            {
                                if (!newNeighbourIds.Contains(oldNeighbourId))
                                {
                                    connectionManager.Disconnect(part.ContentItem.Id, oldNeighbourId);
                                }
                                else newNeighbourIds.Remove(oldNeighbourId); // So newNeighbourIds will contain only really new node ids
                            }

                            foreach (var neighbourId in newNeighbourIds)
                            {
                                connectionManager.Connect(part.ContentItem.Id, neighbourId);
                            }
                        });
                }
                else
                {
                    ProcessLabels(neighbourValues.AddLabels, nodeManager, addNeighbours =>
                        {
                            foreach (var neighbour in addNeighbours)
                            {
                                connectionManager.Connect(part, neighbour);
                            }
                        });

                    ProcessLabels(neighbourValues.RemoveLabels, nodeManager, removeNeighbours =>
                    {
                        foreach (var neighbour in removeNeighbours)
                        {
                            connectionManager.Disconnect(part, neighbour);
                        }
                    });
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

        private static void ProcessLabels(string labels, INodeManager nodeManager, Action<IEnumerable<ContentItem>> processor)
        {
            if (string.IsNullOrEmpty(labels)) return;

            var trimmedLabels = labels.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(label => label.Trim()).ToArray();
            var neighbours = nodeManager.GetByLabelQuery(trimmedLabels).List();

            if (trimmedLabels.Length != neighbours.Count()) return;

            processor(neighbours);
        }
    }
}