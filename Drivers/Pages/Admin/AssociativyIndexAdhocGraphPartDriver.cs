using Associativy.Administration.Models.Pages.Admin;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AssociativyIndexAdhocGraphPartDriver : ContentPartDriver<AssociativyIndexAdhocGraphPart>
    {
        private readonly IContentManager _contentManager;


        public AssociativyIndexAdhocGraphPartDriver(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }


        protected override DriverResult Display(AssociativyIndexAdhocGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociativyIndexAdhocGraph",
            () =>
            {
                part.CreateAdhocGraphRouteValues = _contentManager.GetItemMetadata(_contentManager.New("AssociativyGraph")).CreateRouteValues;

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/IndexAdhocGraph",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}