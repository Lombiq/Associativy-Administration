using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.Frontends.EventHandlers;
using Orchard.Environment.Extensions;
using Associativy.Administration.Services;
using Orchard;
using Orchard.ContentManagement;
using Piedone.HelpfulLibraries.Contents.DynamicPages;
using Associativy.Frontends.Models;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class AuthorizationFrontendEngineEventHandler : IAssociativyFrontendEngineEventHandler
    {
        private readonly IFrontendAuthorizer _frontendAuthorizationService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public AuthorizationFrontendEngineEventHandler(
            IFrontendAuthorizer frontendAuthorizationService,
            IWorkContextAccessor workContextAccessor)
        {
            _frontendAuthorizationService = frontendAuthorizationService;
            _workContextAccessor = workContextAccessor;
        }

        public void OnPageInitializing(IContent page)
        {
        }

        public void OnPageInitialized(IContent page)
        {
        }

        public void OnPageBuilt(IContent page)
        {
        }

        public void OnAuthorization(PageAutorizationContext authorizationContext)
        {
            authorizationContext.Granted = _frontendAuthorizationService.IsAuthorizedToView(
                _workContextAccessor.GetContext().CurrentUser,
                authorizationContext.Page.As<IEngineConfigurationAspect>().GraphContext);
        }
    }
}