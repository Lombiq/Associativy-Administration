using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Associativy.Models;
using System.Collections.Generic;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociativyNodeManagementPart : ContentPart
    {
        public IEnumerable<IAssociativyGraphDescriptor> GraphDescriptors { get; set; }
        public Dictionary<string, string> NeighbourLabels { get; set; }
    }
}