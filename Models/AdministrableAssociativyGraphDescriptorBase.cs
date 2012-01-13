using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions;
using Associativy.Models;
using Piedone.HelpfulLibraries.DependencyInjection;
using Associativy.Services;

namespace Associativy.Administration.Models
{
    [OrchardFeature("Associativy.Administration")]
    public abstract class AdministrableAssociativyGraphDescriptorBase<TNodeToNodeConnectorRecord> : AssociativyGraphDescriptorBase<TNodeToNodeConnectorRecord>
        where TNodeToNodeConnectorRecord : INodeToNodeConnectorRecord, new()
    {
        public override string[] ContentTypes
        {
            get
            {
                // Lazy-loading part
                return base.ContentTypes;
            }
        }

        public AdministrableAssociativyGraphDescriptorBase(IResolve<IConnectionManager<TNodeToNodeConnectorRecord>> connectionManagerResolver)
            : base(connectionManagerResolver)
        {
        }
    }
}