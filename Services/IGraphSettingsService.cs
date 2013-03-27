using Associativy.Administration.Models;
using Orchard;

namespace Associativy.Administration.Services
{
    public interface IGraphSettingsService : IDependency
    {
        IGraphSettings GetSettings(string graphName);
    }
}
