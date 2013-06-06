using System;
using Piedone.HelpfulLibraries.KeyValueStore;

namespace Associativy.Administration.Services
{
    public class GraphSettingsService : IGraphSettingsService
    {
        private const string KeyPrefix = "Associativy.Administration.GraphSettings.";

        private readonly IKeyValueStore _keyValueStore;


        public GraphSettingsService(IKeyValueStore keyValueStore)
        {
            _keyValueStore = keyValueStore;
        }
	
			
        public void Set(string graphName, object settings)
        {
            _keyValueStore.Set(Key(settings.GetType(), graphName), settings);
        }

        public T Get<T>(string graphName)
        {
            return _keyValueStore.Get<T>(Key(typeof(T), graphName));
        }


        public static string Key(Type type, string graphName)
        {
            return KeyPrefix + type.FullName + "." + graphName;
        }
    }
}