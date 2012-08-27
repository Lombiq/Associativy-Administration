using Associativy.Administration.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Migrations
{
    [OrchardFeature("Associativy.Administration")]
    public class BaseMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(typeof(AssociativyNodeManagementPart).Name,
                builder => builder.Attachable());

            SchemaBuilder.CreateTable(typeof(GraphSettingsRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("GraphName", column => column.NotNull().Unique().WithLength(1024))
                    .Column<bool>("UseCache")
                    .Column<int>("InitialZoomLevel")
                    .Column<int>("ZoomLevelCount")
                    .Column<int>("MaxDistance")
            );

            SchemaBuilder.AlterTable(typeof(GraphSettingsRecord).Name,
                table => table
                    .CreateIndex("GraphName", new string[] { "GraphName" })
            );


            return 1;
        }
    }
}