using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AssociativyGraphPartRecord : ContentPartRecord
    {
        public virtual string GraphName { get; set; }
        public virtual string DisplayGraphName { get; set; }
        public virtual string ContainedContentTypes { get; set; }
    }
}