using Associativy.Extensions;
using Associativy.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using Associativy.Administration.Models;

namespace Associativy.Administration.Migrations
{
    [OrchardFeature("Associativy.Administration")]
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(typeof(AssociativyNodeManagementPart).Name,
                builder => builder.Attachable());


            return 1;
        }

        //public int UpdateFrom1()
        //{


        //    return 2;
        //}
    }
}