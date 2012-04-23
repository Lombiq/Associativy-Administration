using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace Associativy.Administration.Models.Admin
{
    [OrchardFeature("Associativy.Administration")]
    public class AssociativyIndexPart : ContentPart
    {
        public int GraphCount { get; set; }
    }
}