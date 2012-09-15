using System.Linq;
using Associativy.Administration.Models;
using Associativy.Frontends;
using Associativy.Frontends.Models.Pages.Frontends;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration")]
    public class SettingsFrontendEngineEventHandler : IPageEventHandler
    {
        private readonly IRepository<GraphSettingsRecord> _settingsRepository;

        public SettingsFrontendEngineEventHandler(IRepository<GraphSettingsRecord> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public void OnPageInitializing(PageContext pageContext)
        {
        }

        public void OnPageInitialized(PageContext pageContext)
        {
            if (pageContext.Group != FrontendsPageConfigs.Group) return;

            var commonPart = pageContext.Page.As<AssociativyFrontendCommonPart>();
            var settings = _settingsRepository.Fetch(record => record.GraphName == commonPart.GraphContext.GraphName).SingleOrDefault();
            if (settings == null) return;
            var customMindSettings = settings.AsMindSettings();
            commonPart.MindSettings.UseCache = customMindSettings.UseCache;
            commonPart.MindSettings.ZoomLevel = customMindSettings.ZoomLevel;
            commonPart.MindSettings.ZoomLevelCount = customMindSettings.ZoomLevelCount;
            commonPart.MindSettings.MaxDistance = customMindSettings.MaxDistance;
        }

        public void OnPageBuilt(PageContext pageContext)
        {
        }

        public void OnAuthorization(PageAutorizationContext authorizationContext)
        {
        }
    }
}