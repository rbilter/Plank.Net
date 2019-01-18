using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PagedList;
using Plank.Net.Data;
using Plank.Net.Managers;
using Plank.Net.Tests.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Tests.Managers
{
    [TestClass]
    public class EntityManagerTests
    {
        #region MEMBERS

        private readonly Mock<ILogger<ParentEntity>> _logger;

        #endregion

        #region CONSTRUCTORS

        public EntityManagerTests()
        {
            _logger = new Mock<ILogger<ParentEntity>>();
        }

        #endregion

        #region METHODS

        [TestMethod]
        public void Constructor_RepositoryNull_ArgumentNullException()
        {
            // Arrange

            // Act
            try
            {
                var manager = new EntityManager<ParentEntity>(null, _logger.Object);
                Assert.Fail("ArgumentNullException should have been thrown for repository.");
            }
            // Assert
            catch (ArgumentNullException e)
            {
                var msg = "Value cannot be null.\r\nParameter name: repository";
                Assert.AreEqual(msg, e.Message);
            }
        }

        [TestMethod]
        public void Constructor_LoggerNull_ArgumentNullException()
        {
            // Arrange
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            try
            {
                var manager = new EntityManager<ParentEntity>(repo.Object, null);
                Assert.Fail("ArgumentNullException should have been thrown for logger.");
            }
            // Assert
            catch (ArgumentNullException e)
            {
                var msg = "Value cannot be null.\r\nParameter name: logger";
                Assert.AreEqual(msg, e.Message);
            }
        }

        [TestMethod]
        public async Task Create_EntityValid_Created()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.CreateAsync(item)).Returns(Task.FromResult(item));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.CreateAsync(item);

            // Assert
            Assert.IsTrue(result.ValidationResults.IsValid);
            Assert.AreEqual(item.Id, result.Item.Id);
            repo.Verify(m => m.CreateAsync(item), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Create_EntityNotValid_NotCreated()
        {
            // Arrange
            var item       = TestHelper.GetParentEntity();
            item.FirstName = string.Empty;

            var repo       = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await  manager.CreateAsync(item);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            repo.Verify(m => m.CreateAsync(item), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Create_ChildEntityNotValid_NotCreated()
        {
            // Arrange
            var invalidChild     = TestHelper.GetChildOne();
            invalidChild.Address = string.Empty;

            var item = TestHelper.GetParentEntity();
            item.ChildOne = new List<ChildOne>
            {
                TestHelper.GetChildOne(),
                invalidChild
            };

            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.CreateAsync(item);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            repo.Verify(m => m.CreateAsync(item), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Create_EntityNull_NotCreated()
        {
            // Arrange
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.CreateAsync(null);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("ParentEntity cannot be null.", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.CreateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Create_ValidatorHasFailResult_NotCreated()
        {
            // Arrange
            var entity = new GrandParentEntity();
            var repo   = new Mock<IEntityRepository<GrandParentEntity>>();
            var logger = new Mock<ILogger<GrandParentEntity>>();

            // Act
            var manager = new EntityManager<GrandParentEntity>(repo.Object, logger.Object);
            var result = await manager.CreateAsync(entity);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was a problem", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.CreateAsync(It.IsAny<GrandParentEntity>()), Times.Never());
            logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Create_ValidatorHasFailResultOnChildEntity_NotCreated()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();
            entity.ChildOne = new List<ChildOne> { TestHelper.GetChildOne() };
            entity.ChildTwo = new List<ChildTwo> { TestHelper.GetChildTwo() };
            var repo   = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.CreateAsync(entity);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was a problem", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.CreateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Create_RepositoryThrowException_NotCreated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.CreateAsync(item)).Throws(new DataException("Error"));

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.CreateAsync(item);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once);
        }

        [TestMethod]
        public async Task Delete_EntityExists_Deleted()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var id   = 1;
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.DeleteAsync(id)).Returns(Task.FromResult(id));
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var results = await manager.DeleteAsync(id);

            // Assert
            Assert.IsTrue(results.ValidationResults.IsValid);
            Assert.AreEqual(id, results.Id);
            repo.Verify(m => m.GetAsync(id), Times.Once());
            repo.Verify(m => m.DeleteAsync(id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task Delete_EntityNotFound_NotDeleted()
        {
            // Arrange
            ParentEntity item = null;
            var id   = 1;
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var results = await manager.DeleteAsync(id);

            // Assert
            Assert.IsTrue(results.ValidationResults.IsValid);
            repo.Verify(m => m.GetAsync(id), Times.Once());
            repo.Verify(m => m.DeleteAsync(id), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task Delete_RepositoryThrowsException_NotDeleted()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var id   = 1;
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.DeleteAsync(id)).Throws(new DataException("Error"));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var results = await manager.DeleteAsync(id);

            // Assert
            Assert.IsFalse(results.ValidationResults.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", results.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", results.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.GetAsync(id), Times.Once());
            repo.Verify(m => m.DeleteAsync(id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once());
        }

        [TestMethod]
        public async Task Get_EntityFoundById_ItemReturned()
        {
            // Arrange
            var id   = 1;
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.GetAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            repo.Verify(m => m.GetAsync(id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public async Task Get_RepositoryThrowsException_IsValidFalse()
        {
            // Arrange
            var id = 1;
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Throws(new DataException("Error"));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.GetAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", result.Message);
            repo.Verify(m => m.GetAsync(id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once());
        }

        [TestMethod]
        public async Task Search_NoCriteria_PageReturned()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var item = TestHelper.GetParentEntity();
            var list = new List<ParentEntity>() { item, item }.ToPagedList(1, 10);
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), pageNumber, pageSize)).Returns(Task.FromResult(list));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.SearchAsync(null, pageNumber, pageSize);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.IsFirstPage);
            Assert.IsTrue(result.IsLastPage);
            Assert.IsFalse(result.HasNextPage);
            Assert.IsFalse(result.HasPreviousPage);
            Assert.AreEqual(2, result.Items.Count());
            Assert.AreEqual(1, result.PageNumber);
            Assert.AreEqual(10, result.PageSize);
            Assert.AreEqual(2, result.TotalItemCount);
            repo.Verify(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), 1, 10), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Search_RepositoryThrowsException_IsValidFalse()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize   = 10;
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), pageNumber, pageSize)).Throws(new DataException("Error"));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.SearchAsync(null, pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", result.Message);
            repo.Verify(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), pageNumber, pageSize), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once());

        }

        [TestMethod]
        public async Task Update_EntityValid_Updated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.UpdateAsync(item)).Returns(Task.FromResult(item));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            Assert.IsTrue(result.ValidationResults.IsValid);
            Assert.AreEqual(item.Id, result.Item.Id);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(item), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Update_EntityNotValid_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            item.FirstName = null;

            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            repo.Verify(m => m.UpdateAsync(item), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Update_EntityNull_NotUpdated()
        {
            // Arrange
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(null);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("ParentEntity cannot be null.", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.UpdateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Update_EntityNotFound_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            ParentEntity rItem = null;
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(rItem));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("Item could not be found.", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Update_ValidatorHasFailResult_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            item.ChildOne = new List<ChildOne> { TestHelper.GetChildOne() };
            item.ChildTwo = new List<ChildTwo> { TestHelper.GetChildTwo() };
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was a problem", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.UpdateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Update_RepositoryThrowsException_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.UpdateAsync(item)).Throws(new DataException("Error"));

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(item), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once());
        }

        [TestMethod]
        public async Task Update_PartialUpdateEntityValid_Updated()
        {
            // Arrange
            var item = new ParentEntity { Id = 0, IsActive = false };
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.UpdateAsync(item, p => p.IsActive)).Returns(Task.FromResult(item));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.UpdateAsync(item, p => p.IsActive);

            // Assert
            Assert.IsTrue(result.ValidationResults.IsValid);
            Assert.AreEqual(item.Id, result.Item.Id);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(item, p => p.IsActive), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));

        }

        [TestMethod]
        public async Task Update_PartialUpdateEntityNotFound_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            ParentEntity rItem = null;
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(rItem));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item, p => p.FirstName);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("Item could not be found.", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Update_PartialUpdateRepositoryThrowsException_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.UpdateAsync(item, p => p.FirstName)).Throws(new DataException("Error"));

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.UpdateAsync(item, p => p.FirstName);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(item, p => p.FirstName), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once());
        }

        #endregion
    }
}
