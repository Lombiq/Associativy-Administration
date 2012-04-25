using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System.Web.Routing;

namespace Associativy.Administration.Models.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyIndexUserGraphPart : ContentPart
    {
        public RouteValueDictionary CreateUserGraphRouteValues { get; set; }
    }
}