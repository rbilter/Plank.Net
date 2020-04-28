using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Tests.Models;
using Plank.Net.Validators;

namespace Plank.Net.Tests.Validators
{
    public class PassValidator : PlankValidator<ChildTwo>
    {
        #region METHODS

        public override ValidationResults Validate(ChildTwo item)
        {
            return new ValidationResults();
        }

        #endregion
    }
}
