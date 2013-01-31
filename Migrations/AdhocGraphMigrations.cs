using Associativy.Administration.Models;
using Associativy.Extensions;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Migrations
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphMigrations : DataMigrationImpl
    {
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


            return 2;
        }
    }
}