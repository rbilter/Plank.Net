using Microsoft.Practices.EnterpriseLibrary.Validation;
using Newtonsoft.Json;
using Plank.Net.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace Plank.Net.Utilities
{
    internal static class EntityExtensionMethods
    {
        #region METHODS

        public static List<PropertyInfo> GetProperties(this IEntity item)
        {
            return item.GetType()
                       .GetProperties()
                       .Where(p => p.DeclaringType == item.GetType() && !p.IsDefined(typeof(InversePropertyAttribute), false))
                       .ToList();
        }

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

        public static ValidationResults Validate<TEntity>(this TEntity item) where TEntity : IEntity
        {
            var validator = ValidationFactory.CreateValidator<TEntity>();
            var validation = validator.Validate(item);

            return validation;
        }

        #endregion
    }
}
