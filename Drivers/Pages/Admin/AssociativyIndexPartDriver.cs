using System.Linq;
using Associativy.Administration.Models.Pages.Admin;
using Associativy.GraphDiscovery;
using Associativy.Services;
using Orchard.ContentManagement.Drivers;

namespace Associativy.Administration.Drivers.Pages.Admin
{
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
                part.GraphCount = _associativyServices.GraphManager.FindGraphs(GraphContext.Empty).Count();

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/Index",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}