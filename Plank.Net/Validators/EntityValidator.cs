using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Models;

namespace Plank.Net.Validators
{
    public abstract class EntityValidator<TEntity> : IEntityValidator<TEntity> where TEntity : class, IEntity
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