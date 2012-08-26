using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System.Web.Routing;

namespace Associativy.Administration.Models.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AssociativyIndexAdhocGraphPart : ContentPart
    {
        public RouteValueDictionary CreateAdhocGraphRouteValues { get; set; }
    }
}