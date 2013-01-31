using Associativy.Administration.Models;
using Associativy.Extensions;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data;
using Orchard.Data.Migration;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using System.Linq;

namespace Associativy.Administration.Migrations
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphMigrations : DataMigrationImpl
    {
        private readonly Work<IRepository<AdhocGraphNodeConnector>> _connectorRepository;
        private readonly Work<IContentManager> _contentManagerWork;


        public AdhocGraphMigrations(
            Work<IRepository<AdhocGraphNodeConnector>> connectorRepository,
            Work<IContentManager> contentManagerWork)
        {
            _connectorRepository = connectorRepository;
            _contentManagerWork = contentManagerWork;
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
                    .CreateIndex("Connection", "GraphName", "Node1Id", "Node2Id")
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


            return 2;
        }

        public int UpdateFrom1()
        {

            SchemaBuilder.AlterTable(typeof(AdhocGraphNodeConnector).Name,
                table =>
                {
                    table.AddColumn<string>("GraphName");
                    table.DropIndex("Connection");
                    table.CreateIndex("Connection", "GraphName", "Node1Id", "Node2Id");
                });

            #region Existing graphs migration
            var graphs = _contentManagerWork.Value.Query("AssociativyGraph").List();
            if (graphs.Any())
            {
                // Dealing with only one ad-hoc graph for now
                var graphName = graphs.First().As<AssociativyGraphPart>().GraphName;
                foreach (var connection in _connectorRepository.Value.Table)
                {
                    connection.GraphName = graphName;
                }
            } 
            #endregion


            return 2;
        }
    }
}