using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Data;
using Plank.Net.Tests.Models;

namespace Plank.Net.Tests.Validators
{
    public class FailValidator : IValidator<ChildTwo>
    {
        #region PROPERTIES

        public int Priority { get; set; } = 1;

        #endregion

        #region METHODS

        public ValidationResults Validate(ChildTwo item)
        {
            var result = new ValidationResults();
            result.AddResult(new ValidationResult("There was a problem", item, null, null, null));

            return result;
        }

        public ValidationResults Validate(object entity)
        {
            return Validate(entity as ChildTwo);
        }

        #endregion
    }
}
