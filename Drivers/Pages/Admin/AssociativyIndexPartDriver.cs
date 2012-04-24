using System.Linq;
using Associativy.GraphDiscovery;
using Associativy.Services;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models.Pages.Admin;

namespace Associativy.Administration.Drivers.Pages.Admin
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
            return ContentShape("Pages_AssociativyIndex",
            () =>
            {
                part.GraphCount = _associativyServices.GraphManager.FindDistinctGraphs(new GraphContext()).Count();

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/Index",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}