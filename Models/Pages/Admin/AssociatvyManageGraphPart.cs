using System.Collections.Generic;
using Associativy.GraphDiscovery;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Utilities;

namespace Associativy.Administration.Models.Pages.Admin
{
    public class AssociatvyManageGraphPart : ContentPart
    {
        public IGraphDescriptor GraphDescriptor { get; set; }

        private readonly LazyField<IGraphSettings> _settings = new LazyField<IGraphSettings>();
        public LazyField<IGraphSettings> SettingsField { get { return _settings; } }
        public IGraphSettings GraphSettings
        {
            get { return _settings.Value; }
            set { _settings.Value = value; }
        }

        private readonly LazyField<IEnumerable<ContentTypeDefinition>> _implicitlyCreatableContentTypes = new LazyField<IEnumerable<ContentTypeDefinition>>();
        public LazyField<IEnumerable<ContentTypeDefinition>> ImplicitlyCreatableContentTypesField { get { return _implicitlyCreatableContentTypes; } }
        public IEnumerable<ContentTypeDefinition> ImplicitlyCreatableContentTypes { get { return _implicitlyCreatableContentTypes.Value; } }
    }
}