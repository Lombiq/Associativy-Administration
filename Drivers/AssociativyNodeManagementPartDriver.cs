using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models;
using Associativy.Services;
using Associativy.Models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Associativy.Administration.Drivers
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociativyNodeManagementPartDriver : ContentPartDriver<AssociativyNodeManagementPart>
    {
        private readonly IAssociativyGraphDescriptorLocator _graphDescriptorLocator;
        private readonly IAssociativyServices _associativyServices;

        protected override string Prefix
        {
            get { return "Associativy.AssociativyNodeManagementPart"; }
        }

        public AssociativyNodeManagementPartDriver(
            IAssociativyGraphDescriptorLocator graphDescriptorLocator,
            IAssociativyServices associativyServices)
        {
            _graphDescriptorLocator = graphDescriptorLocator;
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
            foreach (var descriptor in part.GraphDescriptors)
            {
                _associativyServices.GraphDescriptor = descriptor;
                part.NeighbourLabels[descriptor.TechnicalGraphName] = String.Join(", ", _associativyServices.NodeManager.GetManyQuery(_associativyServices.ConnectionManager.GetNeighbourIds(part.Id)).Join<AssociativyNodeLabelPartRecord>().List().Select(node => node.As<AssociativyNodeLabelPart>().Label));
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

            foreach (var descriptor in part.GraphDescriptors)
            {
                _associativyServices.GraphDescriptor = descriptor;

                var newNeighbourLabels = part.NeighbourLabels[descriptor.TechnicalGraphName].Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(label => label.Trim());
                var newNeighbours = _associativyServices.NodeManager.GetMany(newNeighbourLabels);
                
                if (newNeighbourLabels.Count() == newNeighbours.Count()) // All nodes were found
                {
                    var newNeighbourIds = newNeighbours.Select(node => node.Id).ToList();
                    var oldNeighbourIds = _associativyServices.ConnectionManager.GetNeighbourIds(part.Id);

                    foreach (var oldNeighbourId in oldNeighbourIds)
                    {
                        if (!newNeighbourIds.Contains(oldNeighbourId))
                        {
                            _associativyServices.ConnectionManager.Disconnect(part.Id, oldNeighbourId);
                        }
                        else newNeighbourIds.Remove(oldNeighbourId); // So newNeighbourIds will contain only really new node ids
                    }

                    foreach (var neighbourId in newNeighbourIds)
                    {
                        _associativyServices.ConnectionManager.Connect(part.Id, neighbourId);
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
            part.GraphDescriptors = _graphDescriptorLocator.FindGraphDescriptorsForContentType(part.ContentItem.ContentType);
        }
    }
}