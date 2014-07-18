using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.Administration.Models;
using Associativy.Administration.Services;
using Associativy.GraphDiscovery;
using Associativy.Services;
using Orchard.ContentManagement.Handlers;

namespace Associativy.Administration.Handlers
{
    public class ImplicitlyCreatableAssociativyNodeHandler : ContentHandler
    {
        public ImplicitlyCreatableAssociativyNodeHandler(
            Lazy<IGraphManager> graphManagerLazy,
            Lazy<IGraphSettingsService> settingsService,
            Lazy<INodeIndexingService> nodeIndexingServiceLazy)
        {
            OnPublished<IImplicitlyCreatableAssociativyNodeAspect>((context, part) =>
                {
                    if (string.IsNullOrEmpty(part.Label)) return;

                    // Checking which graphs have this node type as the implicitly creatable type configured.
                    foreach (var graph in graphManagerLazy.Value.FindGraphsByContentTypes(context.ContentType))
                    {
                        var settings = settingsService.Value.GetNotNull<GraphSettings>(graph.Name);

                        if (settings.ImplicitlyCreatableContentType != context.ContentType) continue;

                        nodeIndexingServiceLazy.Value.IndexNodeForGraph(graph.Name, part);
                    }
                });
        }
    }
}