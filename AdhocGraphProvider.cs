using System;
using Associativy.Administration.Models;
using Associativy.GraphDiscovery;
using Associativy.Services;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Associativy.Administration.Services;

namespace Associativy.Administration
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphProvider : IGraphProvider, IContentHandler
    {
        private readonly IGraphServicesFactory _graphServicesFactory;
        private readonly IContentManager _contentManager;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        private const string _graphChangedSignal = "Associativy.Administration.AdhocGraphs.GraphsChanged";

        public Localizer T { get; set; }


        public AdhocGraphProvider(
            IGraphServicesFactory<IStandardMind, IAdhocGraphConnectionManager, IStandardPathFinder, IStandardNodeManager, IStandardGraphStatisticsService> graphServicesFactory,
            IContentManager contentManager,
            ICacheManager cacheManager,
            ISignals signals)
        {
            _graphServicesFactory = graphServicesFactory;
            _contentManager = contentManager;
            _cacheManager = cacheManager;
            _signals = signals;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext describeContext)
        {
            var graphs = _cacheManager.Get("Associativy.Administration.AdhocGraph", ctx =>
            {
                ctx.Monitor(_signals.When(_graphChangedSignal));
                return _contentManager.Query("AssociativyGraph").Join<AssociativyGraphPartRecord>().List();
            });

            foreach (var graph in graphs)
            {
                var graphPart = graph.As<AssociativyGraphPart>();
                describeContext.DescribeGraph(
                    graphPart.GraphName,
                    T(graphPart.DisplayGraphName),
                    graphPart.ContainedContentTypes,
                    _graphServicesFactory.Factory);
            }
        }

        public void Activating(ActivatingContentContext context)
        {
        }

        public void Activated(ActivatedContentContext context)
        {
        }

        public void Initializing(InitializingContentContext context)
        {
        }

        public void Creating(CreateContentContext context)
        {
        }

        public void Created(CreateContentContext context)
        {
            TriggerIfGraph(context);
        }

        public void Loading(LoadContentContext context)
        {
        }

        public void Loaded(LoadContentContext context)
        {
        }

        public void Updating(UpdateContentContext context)
        {
        }

        public void Updated(UpdateContentContext context)
        {
            TriggerIfGraph(context);
        }

        public void Versioning(VersionContentContext context)
        {
        }

        public void Versioned(VersionContentContext context)
        {
        }

        public void Publishing(PublishContentContext context)
        {
        }

        public void Published(PublishContentContext context)
        {
            TriggerIfGraph(context);
        }

        public void Unpublishing(PublishContentContext context)
        {
        }

        public void Unpublished(PublishContentContext context)
        {
            TriggerIfGraph(context);
        }

        public void Removing(RemoveContentContext context)
        {
        }

        public void Removed(RemoveContentContext context)
        {
            TriggerIfGraph(context);
        }

        public void Indexing(IndexContentContext context)
        {
        }

        public void Indexed(IndexContentContext context)
        {
        }

        public void Importing(ImportContentContext context)
        {
        }

        public void Imported(ImportContentContext context)
        {
            TriggerIfGraph(context);
        }

        public void Exporting(ExportContentContext context)
        {
        }

        public void Exported(ExportContentContext context)
        {
        }

        public void GetContentItemMetadata(GetContentItemMetadataContext context)
        {
        }

        public void BuildDisplay(BuildDisplayContext context)
        {
        }

        public void BuildEditor(BuildEditorContext context)
        {
        }

        public void UpdateEditor(UpdateEditorContext context)
        {
        }


        private void TriggerIfGraph(ContentContextBase context)
        {
            if (context.ContentType != "AssociativyGraph") return;
            _signals.Trigger(_graphChangedSignal);
        }
    }
}