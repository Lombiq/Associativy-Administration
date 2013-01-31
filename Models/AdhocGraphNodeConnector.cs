using Associativy.Models;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphNodeConnector : NodeToNodeConnectorRecord
    {
        public virtual string GraphName { get; set; }
    }
}