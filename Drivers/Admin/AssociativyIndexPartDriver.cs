using System.Linq;
using Associativy.Administration.Models.Admin;
using Associativy.GraphDiscovery;
using Associativy.Services;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Drivers.Admin
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociativyIndexPartDriver : ContentPartDriver<AssociativyIndexPart>
    {
        private readonly IAssociativyServices _associativyServices;

        public AssociativyIndexPartDriver(IAssociativyServices associativyServices)
        {
            _associativyServices = associativyServices;
        }

        protected override DriverResult Display(AssociativyIndexPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("AssociativyIndex",
            () =>
            {
                part.GraphCount = _associativyServices.GraphManager.FindDistinctGraphs(new GraphContext()).Count();

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Admin/Index",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}