using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.UI.Admin;
using Orchard.Environment.Extensions;
using System.Web.Mvc;
using Orchard;
using Associativy.Administration.Services;
using Orchard.ContentManagement;
using Orchard.Localization;
using Associativy.Administration.EventHandlers;

namespace Associativy.Administration.Controllers
{
    [Admin, OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class AuthorizationAdminController : AdminControllerBase, IUpdateModel
    {
        private readonly IFrontendAuthorizer _frontendAuthorizer;

        public AuthorizationAdminController(
            IOrchardServices orchardServices,
            IAdminEventHandler eventHandler,
            IFrontendAuthorizer frontendAuthorizer)
            : base(orchardServices, eventHandler)
        {
            _frontendAuthorizer = frontendAuthorizer;
        }

        [HttpPost]
        public ActionResult SaveRoles()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs))
                return new HttpUnauthorizedResult();

            _orchardServices.ContentManager.UpdateEditor(NewPage("ManageGraph"), this);

            return null;
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}