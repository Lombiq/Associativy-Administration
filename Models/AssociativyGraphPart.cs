using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;
using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration.UserGraphs")]
    public class AssociativyGraphPart : ContentPart<AssociativyGraphPartRecord>
    {
        [Required]
        public string GraphName
        {
            get { return Record.GraphName; }
            set { Record.GraphName = value; }
        }

        [Required]
        public string DisplayGraphName
        {
            get { return Record.DisplayGraphName; }
            set { Record.DisplayGraphName = value; }
        }

        private IList<string> _containedContentTypes;
        public IList<string> ContainedContentTypes
        {
            get
            {
                if (_containedContentTypes == null)
                {
                    if (Record.ContainedContentTypes == null) _containedContentTypes = new List<string>();
                    else _containedContentTypes = new JavaScriptSerializer().Deserialize<string[]>(Record.ContainedContentTypes).ToList();
                }

                return _containedContentTypes;
            }

            set
            {
                _containedContentTypes = value;
                if (value == null) Record.ContainedContentTypes = null;
                else Record.ContainedContentTypes = new JavaScriptSerializer().Serialize(_containedContentTypes.ToArray());
            }
        }

        private readonly LazyField<IList<ContentType>> _allContentTypes = new LazyField<IList<ContentType>>();
        public LazyField<IList<ContentType>> AllContentTypesField { get { return _allContentTypes; } }
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