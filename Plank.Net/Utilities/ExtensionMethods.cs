using Newtonsoft.Json;
using System.Collections.Generic;

namespace Plank.Net.Utilities
{
    internal static class ExtensionMethods
    {
        #region METHODS

        public static Dictionary<string, string> ToDictionary(this object item)
        {
            var json = ToJson(item);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static string ToJson(this object item, TypeNameHandling handling = TypeNameHandling.Auto)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting            = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling      = handling
            };

            return JsonConvert.SerializeObject(item, settings);
        }

        public static TEntity ToObject<TEntity>(this Dictionary<string, string> dictionary)
        {
            var json = ToJson(dictionary);
            return JsonConvert.DeserializeObject<TEntity>(json);
        }

        #endregion
    }
}
