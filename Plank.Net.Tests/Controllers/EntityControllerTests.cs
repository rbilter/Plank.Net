using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plank.Net.Controllers;
using Plank.Net.Search;
using Plank.Net.Tests.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Plank.Net.Tests.Controllers
{
    [TestClass]
    public class EntityControllerTests
    {
        #region MEMBERS

        private readonly EntityController<ParentEntity> _controller;

        #endregion

        #region CONSTRUCTORS

        public EntityControllerTests()
        {
            _controller = new EntityController<ParentEntity>(new TestDbContext());
        }

        #endregion

        #region TEST METHODS

        [TestMethod]
        public async Task Create_ValidEntity_Created()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();

            // Act
            var response = await _controller.CreateAsync(item);

            // Assert
            response.ValidationResults.IsValid.Should().BeTrue();
            response.ValidationResults.Should().BeEmpty();
            response.Item.Id.Should().Be(item.Id);
        }

        [TestMethod]
        public async Task Delete_EntityExists_Deleted()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();

            // Act
            var created = await _controller.CreateAsync(entity);
            created.ValidationResults.IsValid.Should().BeTrue();

            var deleted = await _controller.DeleteAsync(created.Item.Id);

            // Assert
            deleted.ValidationResults.IsValid.Should().BeTrue();
            deleted.Id.Should().Be(created.Item.Id);
        }

        [TestMethod]
        public async Task Get_EntityFoundById_EntityReturned()
        {
            // Arrange
            var id = TestHelper.GetParentId();

            // Act
            var response = await _controller.GetAsync(id);

            // Assert
            response.IsValid.Should().BeTrue();
            response.Item.Should().NotBeNull();
            response.Item.ChildOne.Should().HaveCount(1);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task Update_EntityValid_Updated()
        {
            // Arrange
            var firstName = TestHelper.GetRandomString(10);
            var lastName  = TestHelper.GetRandomString(20);
            var add       = TestHelper.GetParentEntity();

            // Act
            var response = await _controller.CreateAsync(add);
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
