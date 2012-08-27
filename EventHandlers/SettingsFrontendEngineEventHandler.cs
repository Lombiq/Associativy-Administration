using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Associativy.Frontends.EventHandlers;
using Orchard.Data;
using Associativy.Administration.Models;
using Orchard.ContentManagement;
using Associativy.Frontends.Models.Pages.Frontends;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.Administration.EventHandlers
{
    [OrchardFeature("Associativy.Administration")]
    public class SettingsFrontendEngineEventHandler : IAssociativyFrontendEngineEventHandler
    {
        private readonly IRepository<GraphSettingsRecord> _settingsRepository;

        public SettingsFrontendEngineEventHandler(IRepository<GraphSettingsRecord> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public void OnPageInitializing(IContent page)
        {
        }

        public void OnPageInitialized(IContent page)
        {
            var commonPart = page.As<AssociativyFrontendCommonPart>();
            var settings = _settingsRepository.Fetch(record => record.GraphName == commonPart.GraphContext.GraphName).SingleOrDefault();
            if (settings == null) return;
            var customMindSettings = settings.AsMindSettings();
            commonPart.MindSettings.UseCache = customMindSettings.UseCache;
            commonPart.MindSettings.ZoomLevel = customMindSettings.ZoomLevel;
            commonPart.MindSettings.ZoomLevelCount = customMindSettings.ZoomLevelCount;
            commonPart.MindSettings.MaxDistance = customMindSettings.MaxDistance;
        }

        public void OnPageBuilt(IContent page)
        {
        }

        public void OnAuthorization(PageAutorizationContext authorizationContext)
        {
        }
    }
}