using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plank.Net.Utilities
{
    internal static class ExtensionMethods
    {
        #region METHODS

        public static Dictionary<string, object> AsDictionary(this object item)
        {
            return item.GetType()
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                       .ToDictionary(p => p.Name, p => p.GetValue(item, null));
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
