using FluentAssertions;
using Moq;
using Plank.Net.Controllers;
using Plank.Net.Search;
using Plank.Net.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Plank.Net.Tests.Controllers
{
    public class PlankControllerTests
    {
        #region MEMBERS

        private readonly PlankController<ParentEntity> _controller;

        #endregion

        #region CONSTRUCTORS

        public PlankControllerTests()
        {
            _controller = new PlankController<ParentEntity>(new TestDbContext());
        }

        #endregion

        #region TEST METHODS

        [Fact]
        public async Task Add_ValidEntity_Created()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();

            // Act
            var response = await _controller.AddAsync(item);

            // Assert
            response.ValidationResults.IsValid.Should().BeTrue();
            response.ValidationResults.Should().BeEmpty();
            response.Item.Id.Should().Be(item.Id);
        }

        [Fact]
        public async Task BulkAdd_ValidEntities_Created()
        {
            // Arrange
            var items = new List<ParentEntity>
            {
                TestHelper.GetParentEntity(),
                TestHelper.GetParentEntity()
            };

            // Act
            var response = await _controller.BulkAddAsync(items);

            // Assert
            response.Items.Should().HaveCount(2);
            response.Items.Where(i => i.ValidationResults.IsValid).Should().HaveCount(2);
            response.Items.Where(i => i.Item.Id == items[0].Id).Should().HaveCount(1);
            response.Items.Where(i => i.Item.Id == items[1].Id).Should().HaveCount(1);
        }

        [Fact]
        public async Task Delete_EntityExists_Deleted()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();

            // Act
            var created = await _controller.AddAsync(entity);
            created.ValidationResults.IsValid.Should().BeTrue();

            var deleted = await _controller.DeleteAsync(created.Item.Id);

            // Assert
            deleted.ValidationResults.IsValid.Should().BeTrue();
            deleted.Id.Should().Be(created.Item.Id);
        }

        [Fact]
        public void Search_BuilderNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Func<Task> act = async () => await SearchWithNullBuilder();

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null.\r\nParameter name: builder");

            Task SearchWithNullBuilder()
            {
                return _controller.SearchAsync(null);
            }
        }

        [Fact]
        public async Task Search_EntitiesExist_PageReturned()
        {
            // Arrange
            var builder = new Mock<ISearchBuilder<ParentEntity>>();
            builder.Setup(p => p.PageNumber).Returns(1);
            builder.Setup(p => p.PageSize).Returns(10);

            // Act
            var response = await _controller.SearchAsync(builder.Object);

            // Assert
            response.Items.Should().NotBeEmpty();
            response.PageNumber.Should().Be(1);
            response.PageSize.Should().Be(10);
            response.TotalItemCount.Should().BeGreaterOrEqualTo(response.Items.Count());
            response.IsValid.Should().BeTrue();
            response.IsFirstPage.Should().BeTrue();
            response.HasPreviousPage.Should().BeFalse();
            (response.TotalItemCount <= response.PageSize ? response.IsLastPage == true : response.IsLastPage == false).Should().BeTrue();
            (response.TotalItemCount <= response.PageSize ? response.HasNextPage == false : response.HasNextPage == true).Should().BeTrue();

            builder.Verify(m => m.Build(), Times.Once());
        }

        [Fact]
        public async Task Update_EntityValid_Updated()
        {
            // Arrange
            var firstName = TestHelper.GetRandomString(10);
            var lastName  = TestHelper.GetRandomString(20);
            var add       = TestHelper.GetParentEntity();

            // Act
            var response = await _controller.AddAsync(add);
            response.ValidationResults.IsValid.Should().BeTrue();

            add.FirstName = firstName;
            add.LastName  = lastName;
            response = await _controller.UpdateAsync(add);

            var updated = await _controller.GetAsync(add.Id);

            // Assert
            response.ValidationResults.IsValid.Should().BeTrue();
            updated.Item.FirstName.Should().Be(firstName);
            updated.Item.LastName.Should().Be(lastName);
        }

        #endregion
    }
}