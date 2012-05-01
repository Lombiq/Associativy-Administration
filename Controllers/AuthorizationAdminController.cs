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
using Orchard.UI.Notify;

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
        public void SaveRoles()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageAssociativyGraphs)) return;

            _orchardServices.ContentManager.UpdateEditor(NewPage("ManageGraph"), this);

            if (ModelState.IsValid)
            {
                _orchardServices.Notifier.Information(T("Roles saved successfully."));
            }
            else
            {
                _orchardServices.Notifier.Information(T("There was some error, roles weren't saved."));
            }
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