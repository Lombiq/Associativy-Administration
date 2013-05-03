using System.Linq;
using Associativy.Administration.Models;
using Orchard.Caching.Services;
using Orchard.Data;

namespace Associativy.Administration.Services
{
    public class GraphSettingsService : IGraphSettingsService
    {
        private readonly ICacheService _cacheService;
        private readonly IRepository<GraphSettingsRecord> _repository;

        private const string CacheKeyPrefix = "Associativy.Administration.GraphSettings.";
        private const string GraphSettingsCacheKeyPrefix = CacheKeyPrefix + "Id";


        public GraphSettingsService(ICacheService cacheService, IRepository<GraphSettingsRecord> repository)
        {
            _cacheService = cacheService;
            _repository = repository;
        }


        public IGraphSettings GetSettings(string graphName)
        {
            var id = _cacheService.Get(GraphSettingsCacheKeyPrefix + "." + graphName, () =>
            {
                var record = _repository.Table.Where(r => r.GraphName == graphName).SingleOrDefault();

                if (record == null)
                {
                    record = new GraphSettingsRecord { GraphName = graphName };
                    _repository.Create(record);
                }

                return record.Id;
            });

            return _repository.Get(id);
        }
    }
}