using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Events;
using Orchard.ContentManagement;

namespace Associativy.Administration.EventHandlers
{
    public interface IAdminEventHandler : IEventHandler
    {
        void OnPageInitializing(IContent page);
        void OnPageInitialized(IContent page);
        void OnPageBuilt(IContent page);
    }
}
