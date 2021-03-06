﻿using Orchard.ContentManagement;

namespace Associativy.Administration.Models
{
    /// <summary>
    /// Describes a node that can be implicitly created (i.e. by specifying a label not yet existing)
    /// </summary>
    public interface IImplicitlyCreatableAssociativyNodeAspect : IContent
    {
        string Label { get; set; }
    }
}
