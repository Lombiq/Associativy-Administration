using System.Linq;
using Associativy.GraphDiscovery;
using Associativy.Services;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models.Pages.Admin;
using Orchard.ContentManagement;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyIndexUserGraphPartDriver : ContentPartDriver<AssociativyIndexUserGraphPart>
    {
        private readonly IContentManager _contentManager;

        public AssociativyIndexUserGraphPartDriver(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        protected override DriverResult Display(AssociativyIndexUserGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociativyIndexUserGraph",
            () =>
            {
                part.CreateUserGraphRouteValues = _contentManager.GetItemMetadata(_contentManager.New("AssociativyGraph")).CreateRouteValues;

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/IndexUserGraph",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}