using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Data;
using Plank.Net.Tests.Models;

namespace Plank.Net.Tests.Validators
{
    public class PassValidator : EntityValidator<ChildTwo>
    {
        #region METHODS

        public override ValidationResults Validate(ChildTwo item)
        {
            return new ValidationResults();
        }

        #endregion
    }
}
