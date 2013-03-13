using System.Linq;
using Associativy.Administration.Models;
using Associativy.Administration.Services;
using Associativy.Frontends;
using Associativy.Frontends.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    public class SettingsFrontendEngineEventHandler : IPageEventHandler
    {
        private readonly IGraphSettingsService _settingsService;


        public SettingsFrontendEngineEventHandler(IGraphSettingsService settingsService)
        {
            _settingsService = settingsService;
        }


        public void OnPageInitializing(PageContext pageContext)
        {
        }

        public void OnPageInitialized(PageContext pageContext)
        {
            if (pageContext.Group != FrontendsPageConfigs.Group) return;

            var config = pageContext.Page.As<IEngineConfigurationAspect>();
            var settings = _settingsService.GetSettings(config.GraphDescriptor.Name);
            if (settings == null) return;

            config.MindSettings.UseCache = settings.UseCache;
            config.MindSettings.MaxDistance = settings.MaxDistance;
            config.GraphSettings.InitialZoomLevel = settings.InitialZoomLevel;
            config.GraphSettings.ZoomLevelCount = settings.ZoomLevelCount;
            config.GraphSettings.MaxConnectionCount = settings.MaxConnectionCount;
        }

        public void OnPageBuilt(PageContext pageContext)
        {
        }

        public void OnAuthorization(PageAutorizationContext authorizationContext)
        {
        }
    }
}