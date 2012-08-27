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

        public void OnPageInitializing(FrontendEventContext frontendEventContext)
        {
        }

        public void OnPageInitialized(FrontendEventContext frontendEventContext)
        {
            var commonPart = frontendEventContext.Page.As<AssociativyFrontendCommonPart>();
            var settings = _settingsRepository.Fetch(record => record.GraphName == commonPart.GraphContext.GraphName).SingleOrDefault();
            if (settings == null) return;
            var customMindSettings = settings.AsMindSettings();
            commonPart.MindSettings.UseCache = customMindSettings.UseCache;
            commonPart.MindSettings.ZoomLevel = customMindSettings.ZoomLevel;
            commonPart.MindSettings.ZoomLevelCount = customMindSettings.ZoomLevelCount;
            commonPart.MindSettings.MaxDistance = customMindSettings.MaxDistance;
        }

        public void OnPageBuilt(FrontendEventContext frontendEventContext)
        {
        }

        public void OnAuthorization(FrontendAuthorizationEventContext frontendAuthorizationEventContext)
        {
        }
    }
}