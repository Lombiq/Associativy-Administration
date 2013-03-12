using Associativy.Administration.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Associativy.Administration.Migrations
{
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
                    .Column<int>("MaxConnectionCount")
            ).AlterTable(typeof(GraphSettingsRecord).Name,
                table => table
                    .CreateIndex("GraphName", new string[] { "GraphName" })
            );


            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable(typeof(GraphSettingsRecord).Name,
                table => table
                    .AddColumn<int>("MaxConnectionCount")
            );


            return 2;
        }
    }
}