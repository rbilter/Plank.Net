using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Data;

namespace Plank.Net.Managers
{
    public interface IValidator<T> where T: Entity
    {
        #region METHODS

        ValidationResults Validate(T item);

        #endregion
    }
}
