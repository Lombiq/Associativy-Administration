using Associativy.Administration.Models;
using Associativy.Extensions;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data;
using Orchard.Data.Migration;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using Piedone.HelpfulLibraries.Libraries.Utilities;
using System.Linq;

namespace Associativy.Administration.Migrations
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphMigrations : DataMigrationImpl
    {
        private readonly IMappingsManager _mappingsManager;
        private readonly IRepository<AdhocGraphNodeConnector> _connectorRepository;
        private readonly IContentManager _contentManager;


        public AdhocGraphMigrations(
            IMappingsManager mappingsManager,
            IRepository<AdhocGraphNodeConnector> connectorRepository,
            IContentManager contentManager)
        {
            _mappingsManager = mappingsManager;
            _connectorRepository = connectorRepository;
            _contentManager = contentManager;
        }


        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(AdhocGraphNodeConnector).Name,
                table => table
                    .NodeConnectorRecord()
                    .Column<string>("GraphName")
                );

            // TODO: TEST INDICES as data grows
            SchemaBuilder.AlterTable(typeof(AdhocGraphNodeConnector).Name,
                table => table
                    .CreateIndex("ConnectionsForGraph", "GraphName")
                );

            SchemaBuilder.CreateTable(typeof(AssociativyGraphPartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("GraphName")
                    .Column<string>("DisplayGraphName")
                    .Column<string>("ContainedContentTypes", column => column.Unlimited())
            );

            ContentDefinitionManager.AlterTypeDefinition("AssociativyGraph",
                cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart(typeof(AssociativyGraphPart).Name)
            );


            return 3;
        }

        public int UpdateFrom1()
        {

            SchemaBuilder.AlterTable(typeof(AdhocGraphNodeConnector).Name,
                table =>
                {
                    table.AddColumn<string>("GraphName");
                    table.DropIndex("Connection");
                    table.CreateIndex("ConnectionsForGraph", "GraphName");
                });


            return 2;
        }

        public int UpdateFrom2()
        {
            // Migrating existing graphs
            // This can't be in UpdateFrom1 as the schema change is not yet persisted and can't be used in the same migration step.

            // Somehow mappings.bin is stale at this point...
            _mappingsManager.Clear();

            var graphs = _contentManager.Query("AssociativyGraph").List();
            if (graphs.Any())
            {
                // Dealing with only one ad-hoc graph for now
                var graphName = graphs.First().As<AssociativyGraphPart>().GraphName;
                foreach (var connection in _connectorRepository.Table)
                {
                    connection.GraphName = graphName;
                }
            }


            return 3;
        }
    }
}