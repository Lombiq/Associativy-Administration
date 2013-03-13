using System.Collections.Generic;
using Associativy.GraphDiscovery;
using Orchard.ContentManagement;

namespace Associativy.Administration.Models
{
    public class AssociativyNodeManagementPart : ContentPart
    {
        public IEnumerable<IGraphDescriptor> GraphDescriptors { get; set; }
        public List<NeighbourValues> NeighbourValues { get; set; }
    }

    public class NeighbourValues
    {
        public int NeighbourCount { get; set; }
        public string Labels { get; set; }
        public string AddLabels { get; set; }
        public string RemoveLabels { get; set; }
        public bool ShowLabels { get; set; }
    }
}