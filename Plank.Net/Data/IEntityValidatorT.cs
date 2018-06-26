using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Data
{
    public interface IEntityValidator<T> : IEntityValidator where T : Entity
    {
        #region METHODS

        ValidationResults Validate(T item);

        #endregion
    }
}
