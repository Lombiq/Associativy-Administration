using System.Linq;
using Associativy.Administration.Models;
using Associativy.Administration.Models.Pages.Admin;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AssociativyManageGraphAdhocGraphPartDriver : ContentPartDriver<AssociativyManageGraphAdhocGraphPart>
    {
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _workContextAccessor;


        public AssociativyManageGraphAdhocGraphPartDriver(
            IContentManager contentManager,
            IWorkContextAccessor workContextAccessor)
        {
            _contentManager = contentManager;
            _workContextAccessor = workContextAccessor;
        }


        protected override DriverResult Display(AssociativyManageGraphAdhocGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociativyManageGraphAdhocGraph",
            () =>
            {
                part.AdhocGraph = _contentManager
                    .Query("AssociativyGraph")
                    .Where<AssociativyGraphPartRecord>(record => record.GraphName == _workContextAccessor.GetContext().HttpContext.Request.QueryString["GraphName"])
                    .List()
                    .FirstOrDefault();

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/ManageGraphAdhocGraph",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}