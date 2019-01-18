using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plank.Net.Data;
using Plank.Net.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            _repo.RegisterNext(new EndRepository<ParentEntity>());
        }

        #endregion

        #region TEST METHODS

        [TestMethod]
        public async Task Create_EntityValid_EntityCreated()
        {
            // Arrange
            var entity      = TestHelper.GetParentEntity();
            entity.ChildOne = new List<ChildOne> { TestHelper.GetChildOne() };

            // Act
            await _repo.CreateAsync(entity);
            var created = await _repo.GetAsync(entity.Id);

            // Assert
            Assert.AreEqual(entity.Id, created.Id);
            Assert.AreEqual(entity.ChildOne.First().Id, created.ChildOne.First().Id);
        }

        [TestMethod]
        public async Task Delete_EntityExists_EntityDeleted()
        {
            // Arrange
            var expected = TestHelper.GetParentEntity();

            // Act
            await _repo.CreateAsync(expected);

            await _repo.DeleteAsync(expected.Id);
            var actual = await _repo.GetAsync(expected.Id);

            // Assert
            Assert.IsNull(actual, "Item was not deleted.");
        }

        [TestMethod]
        public async Task Get_EntityExists_EntityReturned()
        {
            // Arrange
            var id = TestHelper.GetParentId();

            // Act
            var entity = await _repo.GetAsync(id);

            // Assert
            Assert.AreEqual(id, entity.Id);
            Assert.AreEqual("Luke", entity.FirstName);
            Assert.AreEqual("Skywalker", entity.LastName);
        }

        [TestMethod]
        public async Task Search_EntitiesFound_ListReturned()
        {
            // Arrange

            // Act
            var result = await _repo.SearchAsync(i => i.FirstName == "Han" && i.LastName == "Solo");

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public async Task Update_EntityExists_EntityUpdated()
        {
            // Arrange
            var expected = TestHelper.GetParentEntity();

            // Act
            await _repo.CreateAsync(expected);

            var firstName      = TestHelper.GetRandomString(10);
            var lastName       = TestHelper.GetRandomString(20);
            expected.FirstName = firstName;
            expected.LastName  = lastName;

            await _repo.UpdateAsync(expected);
            var actual = await _repo.GetAsync(expected.Id);

            // Assert
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(firstName, actual.FirstName);
            Assert.AreEqual(lastName, actual.LastName);
        }

        [TestMethod]
        public async Task Update_PartialUpdate_PropertiesInExpressionUpdated()
        {
            // Arrange
            var added = TestHelper.GetParentEntity();

            // Act
            await _repo.CreateAsync(added);
            Assert.AreEqual(true, added.IsActive);

            var updated = new ParentEntity { Id = added.Id, IsActive = false };
            await _repo.UpdateAsync(updated, p => p.IsActive);
            var actual  = await _repo.GetAsync(updated.Id);

            // Assert
            Assert.AreEqual(added.Id, actual.Id);
            Assert.AreEqual(added.FirstName, actual.FirstName);
            Assert.AreEqual(added.LastName, actual.LastName);
            Assert.AreEqual(false, actual.IsActive);
        }

        #endregion
    }
}
