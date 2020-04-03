using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Plank.Net.Data
{
    internal static class EntityHelper
    {
        #region METHODS

        [SelfValidation]
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
                        results.AddAllResults(validator.Validate(entity));

                        ValidateWithCustomValidators(results, entity);
                    }
                }
                else
                {
                    var validator = ValidationFactory.CreateValidator(property.GetType());
                    results.AddAllResults(validator.Validate(property));

                    ValidateWithCustomValidators(results, property);
                }
            }
        }

        #endregion

        #region PRIVATE METHODS

        private static void ValidateWithCustomValidators(ValidationResults results, object entity)
        {
            var validators = ValidatorFactory.CreateInstance(entity.GetType());
            foreach (var v in validators)
            {
                results.AddAllResults(v.Validate(entity));
            }
        }

        #endregion
    }
}
