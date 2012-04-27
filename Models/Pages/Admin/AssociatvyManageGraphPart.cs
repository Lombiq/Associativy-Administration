using Associativy.GraphDiscovery;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System.Collections.Generic;
using Associativy.Frontends.EngineDiscovery;

namespace Associativy.Administration.Models.Pages.Admin
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociatvyManageGraphPart : ContentPart
    {
        public GraphDescriptor GraphDescriptor { get; set; }
        public int NodeCount { get; set; }
        public int ConnectionCount { get; set; }
        public IEnumerable<EngineDescriptor> FrontendEngines { get; set; }
    }
}