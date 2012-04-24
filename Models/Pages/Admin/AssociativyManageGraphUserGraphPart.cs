using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyManageGraphUserGraphPart : ContentPart
    {
        public IContent UserGraph { get; set; }

        public bool IsUserGraph
        {
            get { return UserGraph != null; }
        }
    }
}