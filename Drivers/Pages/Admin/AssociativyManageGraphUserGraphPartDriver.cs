using System.Linq;
using Associativy.Administration.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models.Pages.Admin;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyManageGraphUserGraphPartDriver : ContentPartDriver<AssociativyManageGraphUserGraphPart>
    {
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _workContextAccessor;

        public AssociativyManageGraphUserGraphPartDriver(
            IContentManager contentManager,
            IWorkContextAccessor workContextAccessor)
        {
            _contentManager = contentManager;
            _workContextAccessor = workContextAccessor;
        }

        protected override DriverResult Display(AssociativyManageGraphUserGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociativyManageGraphUserGraph",
            () =>
            {
                part.UserGraph = _contentManager
                    .Query("AssociativyGraph")
                    .Where<AssociativyGraphPartRecord>(record => record.GraphName == _workContextAccessor.GetContext().HttpContext.Request.QueryString["GraphName"])
                    .List()
                    .First();

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/ManageGraphUserGraph",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}