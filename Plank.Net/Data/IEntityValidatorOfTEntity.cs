using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Data
{
    public interface IEntityValidator<TEntity> : IEntityValidator where TEntity : Entity
    {
        #region METHODS

        ValidationResults Validate(TEntity item);

        #endregion
    }
}
