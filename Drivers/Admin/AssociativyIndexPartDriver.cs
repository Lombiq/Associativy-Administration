using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models.Admin;
using Orchard.ContentManagement.Drivers;
using Associativy.Services;
using Associativy.GraphDiscovery;

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