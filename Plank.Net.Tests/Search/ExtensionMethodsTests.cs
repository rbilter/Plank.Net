using FluentAssertions;
using Plank.Net.Search;
using Plank.Net.Tests.TestHelpers;
using System;
using Xunit;

namespace Plank.Net.Tests.Search
{
    public class ExtensionMethodsTests
    {
        #region TEST METHODS

        [Fact]
        public void And_FirstNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => AndWithFirstNull();

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null.\r\nParameter name: first");

            void AndWithFirstNull()
            {
                ExtensionMethods.And<ParentEntity>(null, p => p.IsActive);
            }
        }

        [Fact]
        public void And_SecondNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => AndWithSecondNull();

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null.\r\nParameter name: second");

            void AndWithSecondNull()
            {
                ExtensionMethods.And<ParentEntity>(p => p.IsActive, null);
            }
        }

        [Fact]
        public void Or_FirstNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => OrWithFirstNull();

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null.\r\nParameter name: first");

            void OrWithFirstNull()
            {
                ExtensionMethods.Or<ParentEntity>(null, p => p.IsActive);
            }
        }

        [Fact]
        public void Or_SecondNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => OrWithSecondNull();

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null.\r\nParameter name: second");

            void OrWithSecondNull()
            {
                ExtensionMethods.Or<ParentEntity>(p => p.IsActive, null);
            }
        }

        #endregion
    }
}
