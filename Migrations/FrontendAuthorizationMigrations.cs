using Associativy.Administration.Models;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Migrations
{
    [OrchardFeature("Associativy.Administration.FrontendAuthorization")]
    public class FrontendAuthorizationMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(FrontendAuthorizationRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("GraphName", column => column.NotNull().Unique())
                    .Column<string>("RolesDefinition", column => column.Unlimited())
            ).AlterTable(typeof(FrontendAuthorizationRecord).Name,
                table => table
                    .CreateIndex("Graph", new string[] { "GraphName" })
            );


            return 1;
        }
    }
}