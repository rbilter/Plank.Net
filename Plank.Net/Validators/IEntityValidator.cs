using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Models;

namespace Plank.Net.Validators
{
    public interface IEntityValidator<TEntity> : IValidator where TEntity : IEntity
    {
        #region METHODS

        ValidationResults Validate(TEntity item);

        #endregion
    }
}
