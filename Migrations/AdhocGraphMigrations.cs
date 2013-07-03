using System.Linq;
using Associativy.Administration.Models;
using Associativy.Extensions;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using Piedone.HelpfulLibraries.Utilities;

namespace Associativy.Administration.Migrations
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphMigrations : DataMigrationImpl
    {
        private readonly IMappingsManager _mappingsManager;
        private readonly IRepository<AdhocGraphNodeConnector> _connectorRepository;
        private readonly IRepository<AssociativyGraphPartRecord> _graphPartRepository;


        public AdhocGraphMigrations(
            IMappingsManager mappingsManager,
            IRepository<AdhocGraphNodeConnector> connectorRepository,
            IRepository<AssociativyGraphPartRecord> graphPartRepository)
        {
            _mappingsManager = mappingsManager;
            _connectorRepository = connectorRepository;
            _graphPartRepository = graphPartRepository;
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

            ContentDefinitionManager.AlterPartDefinition(typeof(AssociativyGraphPart).Name,
                builder => builder
                    .WithDescription("Stores settings of an ad-hoc Associativy graph."));

            ContentDefinitionManager.AlterTypeDefinition("AssociativyGraph",
                cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart(typeof(AssociativyGraphPart).Name)
                );


            return 4;
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

            var graphs = _graphPartRepository.Table;
            if (graphs.Any())
            {
                // Dealing with only one ad-hoc graph for now
                var graphName = graphs.First().GraphName;
                foreach (var connection in _connectorRepository.Table)
                {
                    connection.GraphName = graphName;
                }
            }


            return 3;
        }

        public int UpdateFrom3()
        {
            ContentDefinitionManager.AlterPartDefinition(typeof(AssociativyGraphPart).Name,
                builder => builder
                    .WithDescription("Stores settings of an ad-hoc Associativy graph."));


            return 4;
        }
    }
}