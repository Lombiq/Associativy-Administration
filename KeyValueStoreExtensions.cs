using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.Administration.Models;
using Piedone.HelpfulLibraries.KeyValueStore;

namespace Associativy.Administration
{
    internal static class KeyValueStoreExtensions
    {
        private const string KeyPrefix = "Associativy.Administration.GraphSettings.";


        public static GraphSettings GetGraphSettings(this IKeyValueStore keyValueStore, string graphName)
        {
            var key = KeyPrefix + graphName;

            if (keyValueStore.Exists(key)) return keyValueStore.Get<GraphSettings>(key);
            return GraphSettings.Default;
        }

        public static void SetGraphSettings(this IKeyValueStore keyValueStore, string graphName, GraphSettings settings)
        {
            keyValueStore.Set(KeyPrefix + graphName, settings);
        }
    }
}