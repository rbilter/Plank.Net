using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Data;
using Plank.Net.Tests.Models;

namespace Plank.Net.Tests.Validators
{
    public class PassValidator : IValidator<ChildTwo>
    {
        #region METHODS

        public ValidationResults Validate(ChildTwo item)
        {
            return new ValidationResults();
        }

        public ValidationResults Validate(object entity)
        {
            return Validate(entity as ChildTwo);
        }

        #endregion
    }
}
