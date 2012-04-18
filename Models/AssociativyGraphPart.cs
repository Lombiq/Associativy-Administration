using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public string ContentTypes
        {
            get { return Record.ContentTypes; }
            set { Record.ContentTypes = value; }
        }
    }
}