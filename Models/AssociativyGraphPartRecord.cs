using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyGraphPartRecord : ContentPartRecord
    {
        public virtual string GraphName { get; set; }
        public virtual string DisplayGraphName { get; set; }
        public virtual string ContainedContentTypes { get; set; }
    }
}