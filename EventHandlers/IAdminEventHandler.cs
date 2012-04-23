using Orchard.ContentManagement;
using Orchard.Events;

namespace Associativy.Administration.EventHandlers
{
    public interface IAdminEventHandler : IEventHandler
    {
        void OnPageInitializing(IContent page);
        void OnPageInitialized(IContent page);
        void OnPageBuilt(IContent page);
    }
}
