using FluentAssertions;
using Plank.Net.Tests.TestHelpers;
using System;
using Xunit;

namespace Plank.Net.Tests.Models
{
    public class PlankEntityTests
    {
        #region TEST METHODS

        [Fact]
        public void Validate_ValidationResultsNull_ArgumentNullException()
        {
            // Arrange

            // Act
            var entity = new PlankEntityHelper();
            Action act = () => ValidateWithNullValidationResults();

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null.\r\nParameter name: results");

            void ValidateWithNullValidationResults()
            {
                entity.Validate(null);
            }
        }

        #endregion
    }
}
