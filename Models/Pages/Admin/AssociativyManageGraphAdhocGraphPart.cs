using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AssociativyManageGraphAdhocGraphPart : ContentPart
    {
        public IContent AdhocGraph { get; set; }

        public bool IsAdhocGraph
        {
            get { return AdhocGraph != null; }
        }
    }
}