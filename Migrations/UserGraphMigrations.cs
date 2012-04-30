using Associativy.Administration.Models;
using Associativy.Extensions;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Migrations
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class UserGraphMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateNodeToNodeConnectorRecordTable<UserGraphNodeConnector>();

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


            return 1;
        }
    }
}