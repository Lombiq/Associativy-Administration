using System.Linq;
using Associativy.Administration.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace Associativy.Administration
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyGrapParthHandler : ContentHandler
    {
        public AssociativyGrapParthHandler(
            IRepository<AssociativyGraphPartRecord> repository,
            IContentDefinitionManager contentDefinitionManager)
        {
            Filters.Add(StorageFilter.For(repository));

            OnInitializing<AssociativyGraphPart>((context, part) =>
            {
                part.AllContentTypesField.Loader(() =>
                    {
                        var containedTypes = part.ContainedContentTypes;
                        var types = contentDefinitionManager.ListTypeDefinitions();
                        return types.Select(type => new ContentType { Name = type.Name, IsContained = containedTypes.Contains(type.Name) }).ToList();
                    });
            });
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "AssociativyGraph")
                return;

            base.GetItemMetadata(context);

            context.Metadata.DisplayText = context.ContentItem.As<AssociativyGraphPart>().DisplayGraphName;
        }
    }
}