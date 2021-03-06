﻿using System.Collections.Generic;
using Associativy.GraphDiscovery;
using Piedone.HelpfulLibraries.Serialization;

namespace Associativy.Administration.Services
{
    public class ImportExportService : IImportExportService
    {
        private readonly IGraphManager _graphManager;
        private readonly ISimpleSerializer _simpleSerializer;


        public ImportExportService(
            IGraphManager graphManager,
            ISimpleSerializer simpleSerializer)
        {
            _graphManager = graphManager;
            _simpleSerializer = simpleSerializer;
        }


        public string ExportAllConnections(IGraphContext graphContext)
        {
            var connections = _graphManager.FindGraph(graphContext).Services.ConnectionManager.GetAll(0, int.MaxValue);
            var serializableConnections = new List<int[]>();
            foreach (var connection in connections)
            {
                serializableConnections.Add(new int[] { connection.Node1Id, connection.Node2Id });
            }

            return _simpleSerializer.JsonSerialize(serializableConnections);
        }

        public void ImportConnections(IGraphContext graphContext, string serializationText)
        {
            var connections = _simpleSerializer.JsonDeserialize<List<int[]>>(serializationText);

            foreach (var connection in connections)
            {
                _graphManager.FindGraph(graphContext).Services.ConnectionManager.Connect(connection[0], connection[1]);
            }
        }
    }
}