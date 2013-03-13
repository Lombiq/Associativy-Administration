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

        private readonly LazyField<IGraphSettings> _settings = new LazyField<IGraphSettings>();
        public LazyField<IGraphSettings> SettingsField { get { return _settings; } }
        public IGraphSettings GraphSettings
        {
            get { return _settings.Value; }
            set { _settings.Value = value; }
        }
    }
}