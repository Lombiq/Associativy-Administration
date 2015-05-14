using System.Collections.Generic;
using System.Linq;
using Associativy.Administration.Models;
using Associativy.Administration.Services;
using Associativy.GraphDiscovery;
using Associativy.Services;
using Orchard.Caching.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace Associativy.Administration
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphProvider : IGraphProvider, IContentHandler
    {
        private readonly IGraphServicesFactory _graphServicesFactory;
        private readonly IContentManager _contentManager;
        private readonly ICacheService _cacheService;

        private const string CacheKey = "Associativy.Administration.AdhocGraph";

        public Localizer T { get; set; }


        public AdhocGraphProvider(
            IGraphServicesFactory<IStandardMind, IAdhocGraphConnectionManager, IStandardPathFinder, IStandardNodeManager> graphServicesFactory,
            IContentManager contentManager,
            ICacheService cacheService)
        {
            _graphServicesFactory = graphServicesFactory;
            _contentManager = contentManager;
            _cacheService = cacheService;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext describeContext)
        {
            var graphs = _cacheService.Get(CacheKey, () =>
            {
                return _contentManager
                    .Query(VersionOptions.Published, "AssociativyGraph")
                    .Join<AssociativyGraphPartRecord>()
                    .List<AssociativyGraphPart>()
                    .Select(item => new Graph
                    {
                        GraphName = item.GraphName,
                        DisplayGraphName = item.DisplayGraphName,
                        ContainedContentTypes = item.ContainedContentTypes
                    });
            });

            foreach (var graph in graphs)
            {
                describeContext.DescribeGraph(
                    graph.GraphName,
                    T(graph.DisplayGraphName),
                    graph.ContainedContentTypes,
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

        public void Initialized(InitializingContentContext context)
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

        public void Restoring(RestoreContentContext context)
        {
        }

        public void Restored(RestoreContentContext context)
        {
        }

        public void Destroying(DestroyContentContext context)
        {
        }

        public void Destroyed(DestroyContentContext context)
        {
        }


        private void TriggerIfGraph(ContentContextBase context)
        {
            if (context.ContentType != "AssociativyGraph") return;
            _cacheService.Remove(CacheKey);
        }


        private class Graph
        {
            public string GraphName { get; set; }
            public string DisplayGraphName { get; set; }
            public IList<string> ContainedContentTypes { get; set; }
        }
    }
}