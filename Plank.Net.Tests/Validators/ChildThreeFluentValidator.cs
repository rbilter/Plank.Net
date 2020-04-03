using FluentValidation;
using Plank.Net.Tests.Models;

namespace Plank.Net.Tests.Validators
{
    public class ChildThreeFluentValidator : AbstractValidator<ChildThree>
    {
        #region

        #endregion

        #region CONSTRUCTORS

        public ChildThreeFluentValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0);
        }

        #endregion
    }
}
