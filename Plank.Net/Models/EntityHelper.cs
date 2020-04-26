using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Plank.Net.Models
{
    internal static class EntityHelper
    {
        #region METHODS

        public static void Validate(IEntity item, ValidationResults results)
        {
            var inverseProperties = item.GetType()
                .GetProperties()
                .Where(e => e.IsDefined(typeof(InversePropertyAttribute), false))
                .ToList();

            ValidateWithCustomValidators(results, item);

            foreach (var property in inverseProperties)
            {
                if (property.GetValue(item) is IEnumerable collection)
                {
                    foreach (var entity in collection)
                    {
                        var validator = ValidationFactory.CreateValidator(entity.GetType());

                        var result = validator.Validate(entity);
                        results.AddAllResults(result);

                        if(result.IsValid)
                        {
                            ValidateWithCustomValidators(results, entity);
                        }
                    }
                }
                else
                {
                    var validator = ValidationFactory.CreateValidator(property.GetType());

                    var result = validator.Validate(property);
                    results.AddAllResults(result);

                    if(result.IsValid)
                    {
                        ValidateWithCustomValidators(results, property);
                    }
                }
            }
        }

        #endregion

        #region PRIVATE METHODS

        private static void ValidateWithCustomValidators(ValidationResults results, object entity)
        {
            var validators = Validators.ValidatorFactory.CreateInstance(entity.GetType());
            foreach (var v in validators)
            {
                var result = v.Validate(entity);
                results.AddAllResults(result);

                if(!result.IsValid)
                {
                    break;
                }
            }
        }

        #endregion
    }
}
