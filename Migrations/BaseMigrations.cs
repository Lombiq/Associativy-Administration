using Associativy.Administration.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Localization;

namespace Associativy.Administration.Migrations
{
    public class BaseMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(typeof(ImplicitlyCreatableAssociativyNodePart).Name,
                builder => builder
                    .Attachable()
                    .WithDescription("When attached it's possible to create its content item through Associativy Administration services by specifying a non-existent label."));

            ContentDefinitionManager.AlterPartDefinition(typeof(AssociativyNodeManagementPart).Name,
                builder => builder
                    .Attachable()
                    .WithDescription("Provides functionality to edit the connections of the node in each Associativy graph it's part of."));


            return 3;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterPartDefinition(typeof(ImplicitlyCreatableAssociativyNodePart).Name,
                builder => builder
                    .Attachable()
                    .WithDescription("When attached it's possible to create its content item through Associativy Administration services by specifying a non-existent label."));

            ContentDefinitionManager.AlterPartDefinition(typeof(AssociativyNodeManagementPart).Name,
                builder => builder
                    .WithDescription("Provides functionality to edit the connections of the node in each Associativy graph it's part of."));


            return 2;
        }

        public int UpdateFrom2()
        {
            SchemaBuilder.DropTable("GraphSettingsRecord");
            SchemaBuilder.DropTable("FrontendAuthorizationRecord");

            return 3;
        }
    }
}