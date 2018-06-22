using Newtonsoft.Json;

namespace Plank.Net.Utilities
{
    public static class ExtensionMethods
    {
        #region METHODS

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

        #endregion
    }
}
