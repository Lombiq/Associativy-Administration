using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.Administration.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Associativy.Services;
using Orchard;
using Associativy.GraphDiscovery;
using Associativy.Models.Mind;
using Associativy.Administration.Models.Admin;

namespace Associativy.Administration.Drivers.Admin
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociatvyMageGraphPartDriver : ContentPartDriver<AssociatvyManageGraphPart>
    {
        private readonly IAssociativyServices _associativyServices;
        private readonly IWorkContextAccessor _workContextAccessor;

        public AssociatvyMageGraphPartDriver(
            IAssociativyServices associativyServices,
            IWorkContextAccessor workContextAccessor)
        {
            _associativyServices = associativyServices;
            _workContextAccessor = workContextAccessor;
        }

        protected override DriverResult Display(AssociatvyManageGraphPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("AssociatvyManageGraph",
            () =>
            {
                var graphContext = new GraphContext { GraphName = _workContextAccessor.GetContext().HttpContext.Request.QueryString["GraphName"] };
                var graph = _associativyServices.Mind.GetAllAssociations(graphContext, new MindSettings { ZoomLevelCount = 1 });

                part.GraphDescriptor = _associativyServices.GraphManager.FindGraph(graphContext);
                part.NodeCount = graph.VertexCount;
                part.ConnectionCount = graph.EdgeCount;

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Admin/ManageGraph",
                            Model: part,
                            Prefix: Prefix);
            });
        }
    }
}