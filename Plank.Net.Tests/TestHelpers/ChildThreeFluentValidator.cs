using FluentValidation;
using Plank.Net.Tests.TestHelpers;

namespace Plank.Net.Tests.TestHelpers
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
