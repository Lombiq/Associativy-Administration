using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.UI.Admin;
using Associativy.Administration.EventHandlers;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Mvc;
using Orchard.Localization;

namespace Associativy.Administration.Controllers
{
    [Admin]
    public abstract class AdminControllerBase : Controller
    {
        protected readonly IOrchardServices _orchardServices;
        protected readonly IAdminEventHandler _eventHandler;

        public Localizer T { get; set; }

        protected AdminControllerBase(
            IOrchardServices orchardServices,
            IAdminEventHandler eventHandler)
        {
            _orchardServices = orchardServices;
            _eventHandler = eventHandler;

            T = NullLocalizer.Instance;
        }

        protected IContent NewPage(string pageName)
        {
            var page = _orchardServices.ContentManager.New("Associativy.Administration " + pageName);

            _eventHandler.OnPageInitializing(page);
            _eventHandler.OnPageInitialized(page);

            return page;
        }

        protected ShapeResult PageResult(IContent page)
        {
            return new ShapeResult(this, _orchardServices.ContentManager.BuildDisplay(page));
        }
    }
}