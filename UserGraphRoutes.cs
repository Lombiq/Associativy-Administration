using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;
using Associativy.Models;
using Associativy.GraphDiscovery;
using Associativy.Frontends;
using Orchard.ContentManagement;

namespace Associativy.Administration
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class UserGraphRoutes : FrontendsRoutesProviderBase
    {
        public UserGraphRoutes()
        {
            //RegisterEngineRoute("Associativy/UserGraphs/{action}", "JIT", OrchardKnowledgeGraphProvider.DescribedGraphContext);
        }
    }
}