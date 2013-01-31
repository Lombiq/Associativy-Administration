﻿using Associativy.Administration.Models;
using Associativy.EventHandlers;
using Associativy.GraphDiscovery;
using Associativy.Models;
using Associativy.Services;
using Orchard.Data;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Associativy.Administration.Services
{
    [OrchardFeature("Associativy.Administration.AdhocGraphs")]
    public class AdhocGraphConnectionManager : IAdhocGraphConnectionManager
    {
        protected readonly IRepository<AdhocGraphNodeConnector> _nodeToNodeRecordRepository;
        protected readonly IMemoryConnectionManager _memoryConnectionManager;
        protected readonly IGraphEventHandler _graphEventHandler;


        public AdhocGraphConnectionManager(
            IRepository<AdhocGraphNodeConnector> nodeToNodeRecordRepository,
            IMemoryConnectionManager memoryConnectionManager,
            IGraphEventHandler graphEventHandler)
        {
            _nodeToNodeRecordRepository = nodeToNodeRecordRepository;
            _memoryConnectionManager = memoryConnectionManager;
            _graphEventHandler = graphEventHandler;
        }


        public void TryLoadConnections(IGraphContext graphContext)
        {
            if (_memoryConnectionManager.GetConnectionCount(graphContext) != 0) return;

            foreach (var connector in _nodeToNodeRecordRepository.Table.Where(record => record.GraphName == graphContext.GraphName))
            {
                _memoryConnectionManager.Connect(graphContext, connector.Node1Id, connector.Node2Id);
            }
        }

        public virtual bool AreNeighbours(IGraphContext graphContext, int node1Id, int node2Id)
        {
            TryLoadConnections(graphContext);

            return _memoryConnectionManager.AreNeighbours(graphContext, node1Id, node2Id);
        }

        public virtual void Connect(IGraphContext graphContext, int node1Id, int node2Id)
        {
            TryLoadConnections(graphContext);

            if (AreNeighbours(graphContext, node1Id, node2Id)) return;

            var connector = new AdhocGraphNodeConnector() { Node1Id = node1Id, Node2Id = node2Id, GraphName = graphContext.GraphName };
            _nodeToNodeRecordRepository.Create(connector);
            _memoryConnectionManager.Connect(graphContext, node1Id, node2Id);
            _graphEventHandler.ConnectionAdded(graphContext, node1Id, node2Id);
        }

        public virtual void DeleteFromNode(IGraphContext graphContext, int nodeId)
        {
            TryLoadConnections(graphContext);

            // Since there is no cummulative delete...
            var connectionsToBeDeleted = _nodeToNodeRecordRepository.Fetch(
                    connector => (connector.Node1Id == nodeId || connector.Node2Id == nodeId) && connector.GraphName == graphContext.GraphName);

            foreach (var connector in connectionsToBeDeleted)
            {
                _nodeToNodeRecordRepository.Delete(connector);
            }

            _memoryConnectionManager.DeleteFromNode(graphContext, nodeId);

            _graphEventHandler.ConnectionsDeletedFromNode(graphContext, nodeId);
        }

        public virtual void Disconnect(IGraphContext graphContext, int node1Id, int node2Id)
        {
            TryLoadConnections(graphContext);

            var connectorRecord = 
                _nodeToNodeRecordRepository.Fetch(
                    connector =>
                        (connector.Node1Id == node1Id && connector.Node2Id == node2Id || connector.Node1Id == node2Id && connector.Node2Id == node1Id) &&
                        connector.GraphName == graphContext.GraphName
                ).FirstOrDefault();

            if (connectorRecord == null) return;

            _nodeToNodeRecordRepository.Delete(connectorRecord);

            _memoryConnectionManager.Disconnect(graphContext, node1Id, node2Id);

            _graphEventHandler.ConnectionDeleted(graphContext, node1Id, node2Id);
        }

        public virtual IEnumerable<INodeToNodeConnector> GetAll(IGraphContext graphContext)
        {
            TryLoadConnections(graphContext);

            return _memoryConnectionManager.GetAll(graphContext);
        }

        public virtual IEnumerable<int> GetNeighbourIds(IGraphContext graphContext, int nodeId)
        {
            TryLoadConnections(graphContext);

            return _memoryConnectionManager.GetNeighbourIds(graphContext, nodeId);
        }

        public virtual int GetNeighbourCount(IGraphContext graphContext, int nodeId)
        {
            return _memoryConnectionManager.GetNeighbourCount(graphContext, nodeId);
        }
    }
}