using Associativy.Administration.Models;
using Associativy.Services;
using Orchard;

namespace Associativy.Administration.Services
{
    public interface IAdhocGraphConnectionManager : ISqlConnectionManager<AdhocGraphNodeConnector>, ITransientDependency
    {
    }
}
