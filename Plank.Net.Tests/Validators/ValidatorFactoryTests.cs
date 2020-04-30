using FluentAssertions;
using Plank.Net.Tests.TestHelpers;
using Plank.Net.Validators;
using System.Linq;
using Xunit;

namespace Plank.Net.Tests.Validators
{
    public class ValidatorFactoryTests
    {
        #region TEST METHODS

        [Fact]
        public void CreateInstancePlankTypeValidators()
        {
            // Arrange

            // Act
            var validators = ValidatorFactory.CreateInstance<ChildTwo>();

            // Assert
            validators.Should().HaveCount(2);
            validators.Where(v => v is PassValidator).Should().HaveCount(1);
            validators.Where(v => v is FailValidator).Should().HaveCount(1);
        }

        [Fact]
        public void CreateInstanceFluentValidators()
        {
            // Arrange

            // Act
            var validators = ValidatorFactory.CreateInstance<ChildThree>();

            // Assert
            validators.Should().HaveCount(1);
            validators.First().GetType().Name.Should().Be("FluentValidatorAdapter`1");
        }

        #endregion
    }
}
