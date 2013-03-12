using System.Collections.Generic;
using Associativy.Frontends.EngineDiscovery;
using Associativy.GraphDiscovery;
using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;

namespace Associativy.Administration.Models.Pages.Admin
{
    public class AssociatvyManageGraphPart : ContentPart
    {
        public IGraphDescriptor GraphDescriptor { get; set; }
        public IEnumerable<IEngineDescriptor> FrontendEngines { get; set; }

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

        public int MaxConnectionCount
        {
            get { return SettingsRecord.MaxConnectionCount; }
            set { SettingsRecord.MaxConnectionCount = value; }
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