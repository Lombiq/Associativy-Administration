﻿using System.Collections.Generic;
using Associativy.GraphDiscovery;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Utilities;

namespace Associativy.Administration.Models.Pages.Admin
{
    public class AssociatvyManageGraphPart : ContentPart
    {
        public IGraphDescriptor GraphDescriptor { get; set; }

        private readonly LazyField<GraphSettings> _settings = new LazyField<GraphSettings>();
        public LazyField<GraphSettings> SettingsField { get { return _settings; } }
        public GraphSettings GraphSettings
        {
            get { return _settings.Value; }
            set { _settings.Value = value; }
        }

        private readonly LazyField<IEnumerable<ContentTypeDefinition>> _implicitlyCreatableContentTypes = new LazyField<IEnumerable<ContentTypeDefinition>>();
        public LazyField<IEnumerable<ContentTypeDefinition>> ImplicitlyCreatableContentTypesField { get { return _implicitlyCreatableContentTypes; } }
        public IEnumerable<ContentTypeDefinition> ImplicitlyCreatableContentTypes { get { return _implicitlyCreatableContentTypes.Value; } }
    }
}