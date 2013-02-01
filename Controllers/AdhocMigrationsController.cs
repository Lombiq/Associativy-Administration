using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Associativy.Administration.Models;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.UI.Admin;
using Orchard.Exceptions;

namespace Associativy.Administration.Controllers
{
    [Admin, OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocMigrationsController : Controller
    {
        private readonly IRepository<AdhocGraphNodeConnector> _connectorRepository;
        private readonly IContentManager _contentManager;


        public AdhocMigrationsController(
            IRepository<AdhocGraphNodeConnector> connectorRepository,
            IContentManager contentManager)
        {
            _connectorRepository = connectorRepository;
            _contentManager = contentManager;
        }


        // Mappings.bin should be deleted before running this...
        public string Index()
        {
            try
            {
                var connectionProbe = _connectorRepository.Table.FirstOrDefault();
                if (connectionProbe == null) return "No connections to migrate.";
                if (!string.IsNullOrEmpty(connectionProbe.GraphName)) return "Connections already migrated.";

                var graphs = _contentManager.Query("AssociativyGraph").List();
                if (graphs.Any())
                {
                    // Dealing with only one ad-hoc graph for now
                    var graphName = graphs.First().As<AssociativyGraphPart>().GraphName;
                    foreach (var connection in _connectorRepository.Table)
                    {
                        connection.GraphName = graphName;
                    }
                }

                return "Migration done";
            }
            catch (Exception ex)
            {
                if (ex.IsFatal()) throw;

                return "An error hapened: " + ex.Message;
            }
        }
    }
}