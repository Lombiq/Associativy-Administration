using Associativy.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Utilities;

namespace Associativy.Administration.Models
{
    public class ImplicitlyCreatableAssociativyNodePart : ContentPart, IImplicitlyCreatableAssociativyNodeAspect
    {
        private readonly LazyField<string> _label = new LazyField<string>();
        internal LazyField<string> LabelField { get { return _label; } }
        public string Label
        {
            get { return _label.Value; }
            set { _label.Value = value; }
        }
    }
}