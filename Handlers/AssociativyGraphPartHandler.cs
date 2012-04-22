﻿using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Core.Common.Fields;
using Piedone.HelpfulLibraries.Contents;
using Associativy.Administration.Models;
using Orchard.ContentManagement.MetaData;
using System.Collections.Generic;
using System.Linq;

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