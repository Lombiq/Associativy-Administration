using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Associativy.GraphDiscovery;

namespace Associativy.Administration.ViewModels
{
    [OrchardFeature("Associativy.Administration")]
    public class GraphManagementViewModel
    {
        public GraphDescriptor Graph { get; set; }
    }
}