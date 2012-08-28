using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AssociativyIndexAdhocGraphPart : ContentPart
    {
        public RouteValueDictionary CreateAdhocGraphRouteValues { get; set; }
    }
}