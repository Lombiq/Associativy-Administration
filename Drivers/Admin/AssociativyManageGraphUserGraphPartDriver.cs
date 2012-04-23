using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;
using Orchard;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models.Admin;
using Associativy.Administration.Models;

namespace Associativy.Administration.Drivers.Admin
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
            return ContentShape("AssociativyManageGraphUserGraph",
            () =>
            {
                part.UserGraph = _contentManager
                    .Query("AssociativyGraph")
                    .Where<AssociativyGraphPartRecord>(record => record.GraphName == _workContextAccessor.GetContext().HttpContext.Request.QueryString["GraphName"])
                    .List()
                    .First();

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Admin/ManageGraphUserGraph",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}