using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Data
{
    public abstract class EntityValidator<T> : IEntityValidator<T> where T : Entity
    {
        #region PROPERTIES

        public int Priority { get; set; }

        #endregion

        #region METHODS

        public abstract ValidationResults Validate(T item);

        public ValidationResults Validate(object entity)
        {
            return Validate(entity as T);
        }

        #endregion
    }
}