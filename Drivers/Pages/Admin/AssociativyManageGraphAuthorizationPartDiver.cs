using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement.Drivers;
using Associativy.Administration.Models.Pages.Admin;
using Associativy.Administration.Services;
using Orchard.Roles.Services;
using Associativy.GraphDiscovery;
using Orchard;
using Orchard.ContentManagement;

namespace Associativy.Administration.Drivers.Pages.Admin
{
    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class AssociativyManageGraphAuthorizationPartDiver : ContentPartDriver<AssociativyManageGraphAuthorizationPart>
    {
        private readonly IFrontendAuthorizer _frontendAuthorizer;
        private readonly IRoleService _roleService;
        private readonly IWorkContextAccessor _workContextAccessor;

        protected override string Prefix
        {
            get { return "Associativy.Administration.AssociativyManageGraphAuthorizationPart"; }
        }

        public AssociativyManageGraphAuthorizationPartDiver(
            IFrontendAuthorizer frontendAuthorizer,
            IRoleService roleService,
            IWorkContextAccessor workContextAccessor)
        {
            _frontendAuthorizer = frontendAuthorizer;
            _roleService = roleService;
            _workContextAccessor = workContextAccessor;
        }

        protected override DriverResult Display(AssociativyManageGraphAuthorizationPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociativyManageGraphAuthorization",
            () =>
            {
                var authorizedRoles = _frontendAuthorizer.GetAuthorizedToView(CurrentContext());
                part.Roles = _roleService.GetRoles().Select(role => new RoleEntry { Name = role.Name, IsAuthorized = authorizedRoles.Contains(role.Name) }).ToList();

                return shapeHelper.DisplayTemplate(
                            TemplateName: "Pages/Admin/ManageGraphAuthorization",
                            Model: part,
                            Prefix: Prefix);
            });
        }

        protected override DriverResult Editor(AssociativyManageGraphAuthorizationPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                _frontendAuthorizer.SetAuthorizedToView(CurrentContext(), part.Roles.Where(role => role.IsAuthorized).Select(role => role.Name));
            }

            return Display(part, "Detail", shapeHelper);
        }

        private IGraphContext CurrentContext()
        {
            return new GraphContext { GraphName = _workContextAccessor.GetContext().HttpContext.Request.QueryString["GraphName"] };
        }
    }
}