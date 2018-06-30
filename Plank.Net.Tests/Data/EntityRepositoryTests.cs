using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plank.Net.Data;
using Plank.Net.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plank.Net.Tests.Data
{
    [TestClass]
    public class EntityRepositoryTests
    {
        #region MEMBERS

        private readonly EntityRepository<ParentEntity> _repo;

        #endregion

        #region CONSTRUCTORS

        public EntityRepositoryTests()
        {
            _repo = new EntityRepository<ParentEntity>(new TestDbContext());
        }

        #endregion

        #region TEST METHODS

        [TestMethod]
        public void Create_EntityValid_EntityCreated()
        {
            // Arrange
            var entity      = TestHelper.GetParentEntity();
            entity.ChildOne = new List<ChildOne> { TestHelper.GetChildOne() };

            // Act
            var id      = _repo.Create(entity);
            var created = _repo.Get(id);

            // Assert
            Assert.AreEqual(entity.Id, id);
            Assert.AreEqual(entity.ChildOne.First().Id, created.ChildOne.First().Id);
        }

        [TestMethod]
        public void Delete_EntityExists_EntityDeleted()
        {
            // Arrange
            var expected = TestHelper.GetParentEntity();

            // Act
            _repo.Create(expected);

            var id     = _repo.Delete(expected.Id);
            var actual = _repo.Get(id);

            // Assert
            Assert.IsNull(actual, "Item was not deleted.");
            Assert.AreEqual(expected.Id, id);
        }

        [TestMethod]
        public void Get_EntityExists_EntityReturned()
        {
            // Arrange
            var id = Guid.Parse("8BDE13A5-DB5B-46FC-8437-0E914EBED531");

            // Act
            var entity = _repo.Get(id);

            // Assert
            Assert.AreEqual(id, entity.Id);
            Assert.AreEqual("Luke", entity.FirstName);
            Assert.AreEqual("Skywalker", entity.LastName);
        }

        [TestMethod]
        public void Search_EntitiesFound_ListReturned()
        {
            // Arrange

            // Act
            var result = _repo.Search(i => i.FirstName == "Han" && i.LastName == "Solo");

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void Update_EntityExists_EntityUpdated()
        {
            // Arrange
            var expected = TestHelper.GetParentEntity();

            // Act
            _repo.Create(expected);

            var firstName      = TestHelper.GetRandomString(10);
            var lastName       = TestHelper.GetRandomString(20);
            expected.FirstName = firstName;
            expected.LastName  = lastName;

            var id     = _repo.Update(expected);
            var actual = _repo.Get(id);

            // Assert
            Assert.AreEqual(expected.Id, id);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(firstName, actual.FirstName);
            Assert.AreEqual(lastName, actual.LastName);
        }

        [TestMethod]
        public void Update_PartialUpdate_PropertiesInExpressionUpdated()
        {
            // Arrange
            var added = TestHelper.GetParentEntity();

            // Act
            _repo.Create(added);
            Assert.AreEqual(true, added.IsActive);

            var updated = new ParentEntity { Id = added.Id, IsActive = false };
            var id      = _repo.Update(updated, p => p.IsActive);
            var actual  = _repo.Get(id);

            // Assert
            Assert.AreEqual(added.Id, actual.Id);
            Assert.AreEqual(added.FirstName, actual.FirstName);
            Assert.AreEqual(added.LastName, actual.LastName);
            Assert.AreEqual(false, actual.IsActive);
        }

        #endregion
    }
}
