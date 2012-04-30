using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.Security;
using Associativy.GraphDiscovery;

namespace Associativy.Administration.Services
{
    public interface IFrontendAuthorizer : IDependency
    {
        void SetAuthorizedToView(IGraphContext graphContext, IEnumerable<string> roles);
        IEnumerable<string> GetAuthorizedToView(IGraphContext graphContext);
        bool IsAuthorizedToView(IUser user, IGraphContext graphContext);
    }
}
