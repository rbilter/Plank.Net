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
            Assert.IsTrue(response.ValidationResults.IsValid);
            Assert.AreEqual(0, response.ValidationResults.Count);
            Assert.AreEqual(item.Id, response.Item.Id);
        }

        [TestMethod]
        public async Task Delete_EntityExists_Deleted()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();

            // Act
            var created = await _controller.CreateAsync(entity);
            Assert.IsTrue(created.ValidationResults.IsValid);

            var deleted = await _controller.DeleteAsync(created.Item.Id);

            // Assert
            Assert.IsTrue(deleted.ValidationResults.IsValid);
            Assert.AreEqual(created.Item.Id, deleted.Id);
        }

        [TestMethod]
        public async Task Get_EntityFoundById_EntityReturned()
        {
            // Arrange
            var id = TestHelper.GetParentId();

            // Act
            var response = await _controller.GetAsync(id);

            // Assert
            Assert.IsTrue(response.IsValid);
            Assert.IsNotNull(response.Item);
            Assert.AreEqual(1, response.Item.ChildOne.Count());
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
            Assert.IsTrue(response.Items.Count() > 0);
            Assert.AreEqual(1, response.PageNumber);
            Assert.AreEqual(10, response.PageSize);
            Assert.IsTrue(response.TotalItemCount >= response.Items.Count());
            Assert.IsTrue(response.IsValid);
            Assert.IsTrue(response.IsFirstPage);
            Assert.IsTrue(response.TotalItemCount <= response.PageSize ? response.IsLastPage == true : response.IsLastPage == false);
            Assert.IsTrue(response.TotalItemCount <= response.PageSize ? response.HasNextPage == false : response.HasNextPage == true);
            Assert.IsFalse(response.HasPreviousPage);

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
            Assert.IsTrue(response.ValidationResults.IsValid);

            add.FirstName = firstName;
            add.LastName  = lastName;
            response = await _controller.UpdateAsync(add);

            var updated = await _controller.GetAsync(add.Id);

            // Assert
            Assert.IsTrue(response.ValidationResults.IsValid);
            Assert.AreEqual(firstName, updated.Item.FirstName);
            Assert.AreEqual(lastName, updated.Item.LastName);
        }

        #endregion
    }
}
