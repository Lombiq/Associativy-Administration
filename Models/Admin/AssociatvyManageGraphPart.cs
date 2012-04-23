using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Associativy.GraphDiscovery;
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