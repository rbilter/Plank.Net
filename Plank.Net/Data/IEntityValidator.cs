using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Data
{
    public interface IEntityValidator
    {
        #region PROPERTIES

        int Priority { get; set; }

        #endregion
        
        #region METHODS

        ValidationResults Validate(object entity);

        #endregion
    }
}
