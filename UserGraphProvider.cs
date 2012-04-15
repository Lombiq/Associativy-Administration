using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.GraphDiscovery;
using Associativy.Services;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models;
using Piedone.HelpfulLibraries.DependencyInjection;
using Orchard.ContentManagement;
using Piedone.HelpfulLibraries.Contents;
using Orchard.Core.Common.Fields;

namespace Associativy.Administration
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class UserGraphProvider : GraphProviderBase<IDatabaseConnectionManager<UserGraphNodeConnector>>
    {
        private readonly IContentManager _contentManager;

        public UserGraphProvider(
            IResolve<IDatabaseConnectionManager<UserGraphNodeConnector>> connectionManagerResolver,
            IContentManager contentManager)
            : base(connectionManagerResolver)
        {
            _contentManager = contentManager;
        }

        public override void Describe(DescribeContext describeContext)
        {
            var graphs = _contentManager.Query("AssociativyGraph").List();

            foreach (var graph in graphs)
            {
                describeContext.DescribeGraph(
                    graph.AsField<TextField>("AssociativyGraphFieldsPart", "GraphName").Value,
                    T(graph.AsField<TextField>("AssociativyGraphFieldsPart", "DisplayGraphName").Value),
                    graph.AsField<TextField>("AssociativyGraphFieldsPart", "ContentTypes").Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                    ConnectionManager);
            }
        }
    }
}