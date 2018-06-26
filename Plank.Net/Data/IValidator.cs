using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Data
{
    public interface IValidator
    {
        #region PROPERTIES

        int Priority { get; set; }

        #endregion
        
        #region METHODS

        ValidationResults Validate(object entity);

        #endregion
    }

    public interface IValidator<T> : IValidator where T : Entity
    {
        #region METHODS

        ValidationResults Validate(T item);

        #endregion
    }
}
