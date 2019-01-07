using Newtonsoft.Json;
using System.Collections.Generic;

namespace Plank.Net.Utilities
{
    internal static class ExtensionMethods
    {
        #region METHODS

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

        #endregion
    }
}
