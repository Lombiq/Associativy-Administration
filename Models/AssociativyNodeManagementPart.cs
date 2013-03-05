using System.Collections.Generic;
using Associativy.GraphDiscovery;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociativyNodeManagementPart : ContentPart
    {
        public IEnumerable<IGraphDescriptor> GraphDescriptors { get; set; }
        public Dictionary<IGraphDescriptor, IGraphContext> GraphContexts { get; set; }
        public List<string> NeighbourLabels { get; set; } // This is a simple list so model binding from POST works (doesn't with IGraphProvider as key)
    }
}