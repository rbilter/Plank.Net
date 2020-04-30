using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Tests.TestHelpers;
using Plank.Net.Validators;

namespace Plank.Net.Tests.TestHelpers
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
