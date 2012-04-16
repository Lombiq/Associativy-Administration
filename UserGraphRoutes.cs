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
    public class UserGraphRoutes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[]
            {
                new RouteDescriptor
                {
                    Name = "Associativy.Administration.UserGraphs",
                    Route = new Route(
                        "AssociativyGraphs/{GraphName}/{controller}/{action}",
                        new RouteValueDictionary {
                                                    {"area", "Associativy.Frontends"},
                                                    {"controller", "JITEngine"},
                                                    {"action", "ShowWholeGraph"}
                                                },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                                                    {"area", "Associativy.Frontends"}
                                                },
                        new MvcRouteHandler())
                }
            };
        }
    }
}