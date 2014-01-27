using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;
using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AssociativyGraphPart : ContentPart<AssociativyGraphPartRecord>
    {
        [Required]
        public string GraphName
        {
            get { return Retrieve(x => x.GraphName); }
            set { Store(x => x.GraphName, value); }
        }

        [Required]
        public string DisplayGraphName
        {
            get { return Retrieve(x => x.DisplayGraphName); }
            set { Store(x => x.DisplayGraphName, value); }
        }

        private IList<string> _containedContentTypes;
        public IList<string> ContainedContentTypes
        {
            get
            {
                if (_containedContentTypes == null)
                {
                    if (ContainedContentTypesSerialized == null) _containedContentTypes = new List<string>();
                    else _containedContentTypes = new JavaScriptSerializer().Deserialize<string[]>(ContainedContentTypesSerialized).ToList();
                }

                return _containedContentTypes;
            }

            set
            {
                _containedContentTypes = value;
                if (value == null) ContainedContentTypesSerialized = null;
                else ContainedContentTypesSerialized = new JavaScriptSerializer().Serialize(_containedContentTypes.ToArray());
            }
        }

        public string ContainedContentTypesSerialized
        {
            get { return Retrieve(x => x.ContainedContentTypes); }
            set { Store(x => x.ContainedContentTypes, value); }
        }

        private readonly LazyField<IList<ContentType>> _allContentTypes = new LazyField<IList<ContentType>>();
        internal LazyField<IList<ContentType>> AllContentTypesField { get { return _allContentTypes; } }
        public IList<ContentType> AllContentTypes
        {
            get { return _allContentTypes.Value; }
        }
    }

    public class ContentType
    {
        public string Name { get; set; }
        public bool IsContained { get; set; }
    }
}