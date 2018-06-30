using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plank.Net.Controllers;
using Plank.Net.Tests.Models;
using System;
using System.Linq;

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
        public void Create_ValidEntity_Created()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();

            // Act
            var response = _controller.Create(item);

            // Assert
            Assert.IsTrue(response.ValidationResults.IsValid);
            Assert.AreEqual(0, response.ValidationResults.Count);
            Assert.AreEqual(item.Id, response.Id);
        }

        [TestMethod]
        public void Delete_EntityExists_Deleted()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();

            // Act
            var created = _controller.Create(entity);
            Assert.IsTrue(created.ValidationResults.IsValid);

            var deleted = _controller.Delete(created.Id);

            // Assert
            Assert.IsTrue(deleted.ValidationResults.IsValid);
            Assert.AreEqual(created.Id, deleted.Id);
        }

        [TestMethod]
        public void Get_EntityFoundById_EntityReturned()
        {
            // Arrange

            // Act
            var response = _controller.Get(Guid.Parse("8BDE13A5-DB5B-46FC-8437-0E914EBED531"));

            // Assert
            Assert.IsTrue(response.IsValid);
            Assert.IsNotNull(response.Item);
            Assert.AreEqual(1, response.Item.ChildOne.Count());
        }

        [TestMethod]
        public void Update_EntityValid_Updated()
        {
            // Arrange
            var firstName = TestHelper.GetRandomString(10);
            var lastName  = TestHelper.GetRandomString(20);
            var add       = TestHelper.GetParentEntity();

            // Act
            var response = _controller.Create(add);
            Assert.IsTrue(response.ValidationResults.IsValid);

            add.FirstName = firstName;
            add.LastName  = lastName;
            response = _controller.Update(add);

            var updated = _controller.Get(add.Id);

            // Assert
            Assert.IsTrue(response.ValidationResults.IsValid);
            Assert.AreEqual(firstName, updated.Item.FirstName);
            Assert.AreEqual(lastName, updated.Item.LastName);
        }

        #endregion
    }
}
