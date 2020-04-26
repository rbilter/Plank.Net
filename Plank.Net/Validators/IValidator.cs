using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Validators
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
}
