using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

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

        private IList<string> _contentTypes;
        public IList<string> ContentTypes
        {
            get
            {
                if (_contentTypes == null)
                {
                    _contentTypes = new JavaScriptSerializer().Deserialize<string[]>(Record.ContentTypes).ToList();
                }

                return _contentTypes;
            }

            set
            {
                _contentTypes = value;
                Record.ContentTypes = new JavaScriptSerializer().Serialize(_contentTypes.ToArray());
            }
        }
    }
}