using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Models;

namespace Plank.Net.Data
{
    public interface IEntityValidator<TEntity> : IEntityValidator where TEntity : IEntity
    {
        #region METHODS

        ValidationResults Validate(TEntity item);

        #endregion
    }
}
