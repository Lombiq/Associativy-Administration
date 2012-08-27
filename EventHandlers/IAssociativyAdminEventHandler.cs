﻿using Orchard.ContentManagement;
using Orchard.Events;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    public interface IAssociativyAdminEventHandler : IPageEventHandler
    {
        // See: http://orchard.codeplex.com/workitem/18990
        new void OnPageInitializing(IContent page);
        new void OnPageInitialized(IContent page);
        new void OnPageBuilt(IContent page);
        new void OnAuthorization(PageAutorizationContext authorizationContext);
    }
}