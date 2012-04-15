using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Core.Common.Fields;
using Piedone.HelpfulLibraries.Contents;

namespace Associativy.Administration
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyGraphHandler : ContentHandler
    {
        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "AssociativyGraph")
                return;

            base.GetItemMetadata(context);

            context.Metadata.DisplayText = context.ContentItem.AsField<TextField>("AssociativyGraphFieldsPart", "DisplayGraphName").Value;
        }
    }
}