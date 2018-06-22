using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Plank.Net.Data
{
    public abstract class Entity
    {
        #region CONSTRUCTORS

        public Entity()
            : this(Guid.NewGuid(), DateTime.Now, DateTime.Now)
        {

        }

        public Entity(Guid id, DateTime dateCreated, DateTime dateLastModified)
        {
            Id = id;
            DateCreated = dateCreated;
            DateLastModified = dateLastModified;
        }

        #endregion

        #region PROPERTIES

        public Guid Id { get; set; }

        public abstract Guid ParentId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }

        #endregion

        #region METHODS

        [SelfValidation]
        protected virtual void Validate(ValidationResults results)
        {
            var inverseProperties = this.GetType()
                .GetProperties()
                .Where(e => e.IsDefined(typeof(InversePropertyAttribute), false))
                .ToList();

            foreach (var property in inverseProperties)
            {
                var collection = property.GetValue(this) as IEnumerable;
                if (collection != null)
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

        private void ValidateWithCustomValidators(ValidationResults results, object entity)
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
