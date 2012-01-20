using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Associativy.Models;
using System.Collections.Generic;
using Associativy.GraphDiscovery;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociativyNodeManagementPart : ContentPart
    {
        // Ide kellenének a base64 contextek
        public IEnumerable<GraphDescriptor> GraphDescriptors { get; set; }
        public Dictionary<string, string> NeighbourLabels { get; set; }
    }
}