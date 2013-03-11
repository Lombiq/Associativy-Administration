using Associativy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Associativy.Administration.Models;
using Orchard;

namespace Associativy.Administration.Services
{
    public interface IAdhocGraphConnectionManager : ISqlConnectionManager<AdhocGraphNodeConnector>, IGraphStatisticsService, ITransientDependency
    {
    }
}
