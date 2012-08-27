using Associativy.GraphDiscovery;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System.Collections.Generic;
using Associativy.Frontends.EngineDiscovery;
using Orchard.Core.Common.Utilities;

namespace Associativy.Administration.Models.Pages.Admin
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociatvyManageGraphPart : ContentPart
    {
        public GraphDescriptor GraphDescriptor { get; set; }
        public int NodeCount { get; set; }
        public int ConnectionCount { get; set; }
        public IEnumerable<EngineDescriptor> FrontendEngines { get; set; }

        #region Settings
        public bool UseCache
        {
            get { return SettingsRecord.UseCache; }
            set { SettingsRecord.UseCache = value; }
        }

        public int InitialZoomLevel
        {
            get { return SettingsRecord.InitialZoomLevel; }
            set { SettingsRecord.InitialZoomLevel = value; }
        }

        public int ZoomLevelCount
        {
            get { return SettingsRecord.ZoomLevelCount; }
            set { SettingsRecord.ZoomLevelCount = value; }
        }

        public int MaxDistance
        {
            get { return SettingsRecord.MaxDistance; }
            set { SettingsRecord.MaxDistance = value; }
        }

        private readonly LazyField<GraphSettingsRecord> _settingsRecord = new LazyField<GraphSettingsRecord>();
        public LazyField<GraphSettingsRecord> SettingsRecordField { get { return _settingsRecord; } }
        public GraphSettingsRecord SettingsRecord
        {
            get { return _settingsRecord.Value; }
        }
        #endregion
    }
}