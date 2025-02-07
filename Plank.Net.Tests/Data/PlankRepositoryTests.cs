﻿using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentAssertions;
using Plank.Net.Data;
using Plank.Net.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Plank.Net.Tests.Data
{
    public class PlankRepositoryTests
    {
        #region MEMBERS

        private readonly PlankRepository<ParentEntity> _repo;

        #endregion

        #region CONSTRUCTORS

        public PlankRepositoryTests()
        {
            _repo = new PlankRepository<ParentEntity>(new TestDbContext());
            _repo.RegisterNext(new EndRepository<ParentEntity>());
        }

        #endregion

        #region TEST METHODS

        [Fact]
        public void Constructor_DbContextNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => CreateRespoistoryWithNullContext();

            // Assert
            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null.\r\nParameter name: context");

            void CreateRespoistoryWithNullContext()
            {
                _ = new PlankRepository<ChildOne>(null);
            }
        }

        [Fact]
        public async Task Add_EntityValid_EntityCreated()
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

        [Fact]
        public async Task BulkAdd_EntitiesValid_EntitiesCreated()
        {
            // Arrange
            var entities = new List<ParentEntity>
            {
                TestHelper.GetParentEntity(),
                TestHelper.GetParentEntity()
            };

            // Act
            await _repo.BulkAddAsync(entities);
            var ids = entities.Select(e => e.Id).ToList();
            var created = (await _repo.SearchAsync(e => ids.Contains(e.Id))).ToList();

            // Assert
            created.Should().HaveCount(2);
            entities.Select(e => e.Id == created[0].Id).Should().NotBeEmpty();
            entities.Select(e => e.Id == created[1].Id).Should().NotBeEmpty();
        }

        [Fact]
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

        [Fact]
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

        [Fact]
        public async Task Search_EntitiesFound_ListReturned()
        {
            // Arrange

            // Act
            var result = await _repo.SearchAsync(i => i.FirstName == "Han" && i.LastName == "Solo");

            // Assert
            result.Should().HaveCount(1);
            result[0].ChildOne.Should().BeNull();
        }

        [Fact]
        public async Task Search_IncludesProvided_ListReturned()
        {
            // Arrange
            var includes = new List<Expression<Func<ParentEntity, object>>>
            {
                i => i.ChildOne
            };

            // Act
            var result = await _repo.SearchAsync(i => i.FirstName == "Han" && i.LastName == "Solo", includes);

            // Assert
            result.Should().HaveCount(1);
            result[0].ChildOne.Should().NotBeNull();
        }

        [Fact]
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
