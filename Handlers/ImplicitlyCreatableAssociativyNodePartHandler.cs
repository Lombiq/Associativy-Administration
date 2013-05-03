using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.Administration.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement;
using Associativy.Models;

namespace Associativy.Administration.Handlers
{
    public class ImplicitlyCreatableAssociativyNodePartHandler : ContentHandler
    {
        public ImplicitlyCreatableAssociativyNodePartHandler()
        {
            OnActivated<ImplicitlyCreatableAssociativyNodePart>((context, part) =>
                {
                    var labelAspect = part.As<IAssociativyNodeLabelAspect>();

                    if (labelAspect == null) return;

                    part.LabelField.Loader(() => labelAspect.Label);
                    part.LabelField.Setter(label => labelAspect.Label = label);
                });
        }
    }
}