using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plank.Net.Data;
using System;
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
            var entity = TestHelper.GetParentEntity();

            // Act
            var id = _repo.Create(entity);

            // Assert
            Assert.AreEqual(entity.Id, id);
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

        #endregion
    }
}
