using Associativy.Administration.Models;
using Associativy.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;

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