using FluentAssertions;
using Plank.Net.Data;
using Plank.Net.Tests.Models;
using Plank.Net.Tests.Validators;
using System.Linq;
using Xunit;

namespace Plank.Net.Tests.Data
{
    public class ValidatorFactoryTests
    {
        #region FACTS

        [Fact]
        public void CreateInstanceEntityTypeValidators()
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
        public void CreateInstanceAbstractValidators()
        {
            // Arrange

            // Act
            var validators = ValidatorFactory.CreateInstance<ChildThree>();

            // Assert
            validators.Should().HaveCount(1);
            validators.First().GetType().Name.Should().Be("EntityFluentValidator`1");
        }

        #endregion
    }
}
