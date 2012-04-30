using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using Associativy.Administration.Models.Pages.Admin;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class AuthorizationAdminEventHandler : IAdminEventHandler
    {
        public void OnPageInitializing(IContent page)
        {
            if (page.IsPage("ManageGraph"))
            {
                page.ContentItem.Weld(new AssociativyManageGraphAuthorizationPart());
            }
        }

        public void OnPageInitialized(IContent page)
        {
        }

        public void OnPageBuilt(IContent page)
        {
        }
    }
}