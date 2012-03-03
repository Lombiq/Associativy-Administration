using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Associativy.GraphDiscovery;

namespace Associativy.Administration.Services
{
    public interface IImportExportService : IDependency
    {
        string ExportAllConnections(IGraphContext graphContext);
        void ImportConnections(IGraphContext graphContext, string serializationText);
    }
}
