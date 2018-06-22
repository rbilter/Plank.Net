using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Data
{
    public interface IValidator
    {
        ValidationResults Validate(object entity);
    }

    public interface IValidator<T> : IValidator where T: Entity
    {
        #region METHODS

        ValidationResults Validate(T item);

        #endregion
    }
}
