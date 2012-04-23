using Associativy.GraphDiscovery;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models.Admin
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociatvyManageGraphPart : ContentPart
    {
        public GraphDescriptor GraphDescriptor { get; set; }
        public int NodeCount { get; set; }
        public int ConnectionCount { get; set; }
    }
}