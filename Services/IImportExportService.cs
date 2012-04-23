using Associativy.GraphDiscovery;
using Orchard;

namespace Associativy.Administration.Services
{
    public interface IImportExportService : IDependency
    {
        string ExportAllConnections(IGraphContext graphContext);
        void ImportConnections(IGraphContext graphContext, string serializationText);
    }
}
