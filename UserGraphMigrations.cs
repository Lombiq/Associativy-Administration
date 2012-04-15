using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.Data.Migration;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Associativy.Administration.Models;
using Associativy.Extensions;

namespace Associativy.Administration
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class UserGraphMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateNodeToNodeConnectorRecordTable<UserGraphNodeConnector>();

            ContentDefinitionManager.AlterPartDefinition("AssociativyGraphFieldsPart",
                part => part
                    .WithField("GraphName", field =>
                    {
                        field.OfType("TextField");
                    })
                    .WithField("DisplayGraphName", field =>
                    {
                        field.OfType("TextField");
                    })
                    .WithField("ContentTypes", field =>
                    {
                        field.OfType("TextField");
                    })
            );

            ContentDefinitionManager.AlterTypeDefinition("AssociativyGraph",
                cfg => cfg
                    .WithPart("CommonPart")
                    .WithPart("AssociativyGraphFieldsPart")
                    .Creatable()
            );


            return 1;
        }
    }
}