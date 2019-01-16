using Newtonsoft.Json;
using System.Collections.Generic;

namespace Plank.Net.Utilities
{
    internal static class ExtensionMethods
    {
        #region METHODS

        public static Dictionary<string, object> ToDictionary(this object item)
        {
            if (item == null) { return null; }

            var json = item.ToJson();
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
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

        public static T ToObject<T>(this Dictionary<string, object> dictionary)
        {
            var json = ToJson(dictionary);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion
    }
}
