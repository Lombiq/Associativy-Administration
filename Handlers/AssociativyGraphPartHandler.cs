using System.Linq;
using Associativy.Administration.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.MetaData;
using Orchard.Data;
using Orchard.Environment;
using Orchard.Environment.Extensions;

namespace Associativy.Administration
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AssociativyGrapParthHandler : ContentHandler
    {
        public AssociativyGrapParthHandler(
            IRepository<AssociativyGraphPartRecord> repository,
            Work<IContentDefinitionManager> contentDefinitionManagerWork)
        {
            Filters.Add(StorageFilter.For(repository));

            OnActivated<AssociativyGraphPart>((context, part) =>
                {
                    part.AllContentTypesField.Loader(() =>
                        {
                            var containedTypes = part.ContainedContentTypes;
                            var types = contentDefinitionManagerWork.Value.ListTypeDefinitions();
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