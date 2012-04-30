using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.Frontends.EventHandlers;
using Orchard.Environment.Extensions;
using Associativy.Administration.Services;
using Orchard;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class AuthorizationFrontendEngineEventHandler : IFrontendEngineEventHandler
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

        public void OnPageInitializing(FrontendEventContext frontendEventContext)
        {
        }

        public void OnPageInitialized(FrontendEventContext frontendEventContext)
        {
        }

        public void OnPageBuilt(FrontendEventContext frontendEventContext)
        {
        }

        public void OnAuthorization(FrontendAuthorizationEventContext frontendAuthorizationEventContext)
        {
            frontendAuthorizationEventContext.Granted = _frontendAuthorizationService.IsAuthorizedToView(
                _workContextAccessor.GetContext().CurrentUser,
                frontendAuthorizationEventContext.GraphContext);
        }
    }
}