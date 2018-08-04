using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Data
{
    public abstract class EntityValidator<TEntity> : IEntityValidator<TEntity> where TEntity : Entity
    {
        #region PROPERTIES

        public int Priority { get; set; }

        #endregion

        #region METHODS

        public abstract ValidationResults Validate(TEntity item);

        public ValidationResults Validate(object entity)
        {
            return Validate(entity as TEntity);
        }

        #endregion
    }
}