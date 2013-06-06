using System;
using Orchard;

namespace Associativy.Administration.Services
{
    public interface IGraphSettingsService : IDependency
    {
        T Get<T>(string graphName);
        void Set(string graphName, object settings);
    }


    public static class GraphSettingsServiceExtensions
    {
        public static T GetNotNull<T>(this IGraphSettingsService settingsService, string graphName) where T : new()
        {
            var value = settingsService.Get<T>(graphName);
            if (value == null) return new T();
            return value;
        }
    }
}
