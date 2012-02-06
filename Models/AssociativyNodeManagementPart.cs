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
        public IEnumerable<IGraphProvider> GraphProviders { get; set; }
        public Dictionary<IGraphProvider, IGraphContext> GraphContexts { get; set; }
        public List<string> NeighbourLabels { get; set; } // This is a simple list so model binding from POST works (doesn't with IGraphProvider as key)
    }
}