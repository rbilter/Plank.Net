using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plank.Net.Data;
using Plank.Net.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plank.Net.Tests.Data
{
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
            await _repo.AddAsync(entity);
            var created = await _repo.GetAsync(entity.Id);

            // Assert
            created.Id.Should().Be(entity.Id);
            created.ChildOne.First().Id.Should().Be(entity.ChildOne.First().Id);
        }

        [TestMethod]
        public async Task Delete_EntityExists_EntityDeleted()
        {
            // Arrange
            var expected = TestHelper.GetParentEntity();

            // Act
            await _repo.AddAsync(expected);

            await _repo.DeleteAsync(expected.Id);
            var actual = await _repo.GetAsync(expected.Id);

            // Assert
            actual.Should().BeNull();
        }

        [TestMethod]
        public async Task Get_EntityExists_EntityReturned()
        {
            // Arrange
            var id = TestHelper.GetParentId();

            // Act
            var entity = await _repo.GetAsync(id);

            // Assert
            entity.Id.Should().Be(id);
            entity.FirstName.Should().Be("Luke");
            entity.LastName.Should().Be("Skywalker");
        }

        [TestMethod]
        public async Task Search_EntitiesFound_ListReturned()
        {
            // Arrange

            // Act
            var result = await _repo.SearchAsync(i => i.FirstName == "Han" && i.LastName == "Solo");

            // Assert
            result.Should().HaveCount(1);
        }

        [TestMethod]
        public async Task Update_EntityExists_EntityUpdated()
        {
            // Arrange
            var expected = TestHelper.GetParentEntity();

            // Act
            await _repo.AddAsync(expected);

            var firstName      = TestHelper.GetRandomString(10);
            var lastName       = TestHelper.GetRandomString(20);
            expected.FirstName = firstName;
            expected.LastName  = lastName;

            await _repo.UpdateAsync(expected);
            var actual = await _repo.GetAsync(expected.Id);

            // Assert
            actual.Id.Should().Be(expected.Id);
            actual.FirstName.Should().Be(firstName);
            actual.LastName.Should().Be(lastName);
        }

        #endregion
    }
}
