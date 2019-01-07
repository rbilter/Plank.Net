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

        public static string ToJson(this object item, TypeNameHandling handling = TypeNameHandling.All)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting                 = Formatting.Indented,
                MaxDepth                   = 10,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling      = ReferenceLoopHandling.Serialize,
                TypeNameHandling           = handling
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
