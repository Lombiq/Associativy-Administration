using Associativy.Administration.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Localization;

namespace Associativy.Administration.Migrations
{
    public class BaseMigrations : DataMigrationImpl
    {
        public Localizer T { get; set; }


        public BaseMigrations()
        {
            T = NullLocalizer.Instance;
        }


        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(typeof(ImplicitlyCreatableAssociativyNodePart).Name,
                builder => builder
                    .Attachable()
                    .WithDescription(T("When attached it's possible to create its content item through Associativy Administration services by specifying a non-existent label.").Text));

            ContentDefinitionManager.AlterPartDefinition(typeof(AssociativyNodeManagementPart).Name,
                builder => builder
                    .Attachable()
                    .WithDescription(T("Provides functionality to edit the connections of the node in each Associativy graph it's part of.").Text));

            SchemaBuilder.CreateTable(typeof(GraphSettingsRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("GraphName", column => column.NotNull().Unique().WithLength(1024))
                    .Column<bool>("UseCache")
                    .Column<int>("InitialZoomLevel")
                    .Column<int>("ZoomLevelCount")
                    .Column<int>("MaxDistance")
                    .Column<int>("MaxConnectionCount")
                    .Column<string>("ImplicitlyCreatableContentType", column => column.WithLength(1024))
            ).AlterTable(typeof(GraphSettingsRecord).Name,
                table => table
                    .CreateIndex("GraphName", new string[] { "GraphName" })
            );


            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable(typeof(GraphSettingsRecord).Name,
                table =>
                {
                    table.AddColumn<int>("MaxConnectionCount");
                    table.AddColumn<string>("ImplicitlyCreatableContentType", column => column.WithLength(1024));
                }
            );

            ContentDefinitionManager.AlterPartDefinition(typeof(ImplicitlyCreatableAssociativyNodePart).Name,
                builder => builder
                    .Attachable()
                    .WithDescription(T("When attached it's possible to create its content item through Associativy Administration services by specifying a non-existent label.").Text));

            ContentDefinitionManager.AlterPartDefinition(typeof(AssociativyNodeManagementPart).Name,
                builder => builder
                    .WithDescription(T("Provides functionality to edit the connections of the node in each Associativy graph it's part of.").Text));


            return 2;
        }
    }
}