using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plank.Net.Data;
using Plank.Net.Managers;
using Plank.Net.Tests.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        public void Create_EntityValid_Created()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Create(item)).Returns(item.Id);

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = manager.Create(item);

            // Assert
            Assert.IsTrue(result.ValidationResults.IsValid);
            Assert.AreEqual(item.Id, result.Id);
            repo.Verify(m => m.Create(item), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Create_EntityNotValid_NotCreated()
        {
            // Arrange
            var item       = TestHelper.GetParentEntity();
            item.FirstName = string.Empty;

            var repo       = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = manager.Create(item);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            repo.Verify(m => m.Create(item), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Create_ChildEntityNotValid_NotCreated()
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
            var result  = manager.Create(item);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            repo.Verify(m => m.Create(item), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Create_EntityNull_NotCreated()
        {
            // Arrange
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = manager.Create(null);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("ParentEntity cannot be null.", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.Create(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Create_ValidatorHasFailResult_NotCreated()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();
            entity.ChildOne = new List<ChildOne> { TestHelper.GetChildOne() };
            entity.ChildTwo = new List<ChildTwo> { TestHelper.GetChildTwo() };
            var repo   = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = manager.Create(entity);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was a problem", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.Create(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Create_RepositoryThrowException_NotCreated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Create(item)).Throws(new DataException("Error"));

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = manager.Create(item);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once);
        }

        [TestMethod]
        public void Delete_EntityExists_Deleted()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var id   = Guid.NewGuid();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Delete(id)).Returns(id);
            repo.Setup(m => m.Get(id)).Returns(item);

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var results = manager.Delete(id);

            // Assert
            Assert.IsTrue(results.ValidationResults.IsValid);
            Assert.AreEqual(id, results.Id);
            repo.Verify(m => m.Get(id), Times.Once());
            repo.Verify(m => m.Delete(id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<Guid>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Delete_EntityNotFound_NotDeleted()
        {
            // Arrange
            ParentEntity item = null;
            var id   = Guid.NewGuid();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Get(id)).Returns(item);

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var results = manager.Delete(id);

            // Assert
            Assert.IsTrue(results.ValidationResults.IsValid);
            repo.Verify(m => m.Get(id), Times.Once());
            repo.Verify(m => m.Delete(id), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<Guid>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Delete_RepositoryThrowsException_NotDeleted()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var id   = Guid.NewGuid();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Get(id)).Returns(item);
            repo.Setup(m => m.Delete(id)).Throws(new DataException("Error"));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var results = manager.Delete(id);

            // Assert
            Assert.IsFalse(results.ValidationResults.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", results.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", results.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.Get(id), Times.Once());
            repo.Verify(m => m.Delete(id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<Guid>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once());
        }

        [TestMethod]
        public void Get_EntityFoundById_ItemReturned()
        {
            // Arrange
            var id   = Guid.Empty;
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Get(id)).Returns(item);

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = manager.Get(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            repo.Verify(m => m.Get(id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<Guid>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Get_RepositoryThrowsException_NullReturned()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Get(id)).Throws(new DataException("Error"));

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = manager.Get(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", result.Message);
            repo.Verify(m => m.Get(id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<Guid>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once());
        }

        [TestMethod]
        public void Update_EntityValid_Updated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Get(item.Id)).Returns(item);
            repo.Setup(m => m.Update(item)).Returns(item.Id);

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = manager.Update(item);

            // Assert
            Assert.IsTrue(result.ValidationResults.IsValid);
            Assert.AreEqual(item.Id, result.Id);
            repo.Verify(m => m.Get(item.Id), Times.Once());
            repo.Verify(m => m.Update(item), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Update_EntityNotValid_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            item.FirstName = null;

            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = manager.Update(item);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            repo.Verify(m => m.Update(item), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Update_EntityNull_NotUpdated()
        {
            // Arrange
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = manager.Update(null);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("ParentEntity cannot be null.", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.Update(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Update_EntityNotFound_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            ParentEntity rItem = null;
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Get(item.Id)).Returns(rItem);

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = manager.Update(item);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("Item could not be found.", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.Get(item.Id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Update_ValidatorHasFailResult_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            item.ChildOne = new List<ChildOne> { TestHelper.GetChildOne() };
            item.ChildTwo = new List<ChildTwo> { TestHelper.GetChildTwo() };
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = manager.Update(item);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was a problem", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.Update(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Update_RepositoryThrowsException_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            repo.Setup(m => m.Get(item.Id)).Returns(item);
            repo.Setup(m => m.Update(item)).Throws(new DataException("Error"));

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = manager.Update(item);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.Get(item.Id), Times.Once());
            repo.Verify(m => m.Update(item), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once());
        }

        [TestMethod]
        public void Update_PartialUpdateEntityValid_Updated()
        {
            // Arrange
            var item = new ParentEntity { Id = Guid.Empty, IsActive = false };
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Get(item.Id)).Returns(item);
            repo.Setup(m => m.Update(item, p => p.IsActive)).Returns(item.Id);

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = manager.Update(item, p => p.IsActive);

            // Assert
            Assert.IsTrue(result.ValidationResults.IsValid);
            Assert.AreEqual(item.Id, result.Id);
            repo.Verify(m => m.Get(item.Id), Times.Once());
            repo.Verify(m => m.Update(item, p => p.IsActive), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));

        }

        [TestMethod]
        public void Update_PartialUpdateEntityNotFound_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            ParentEntity rItem = null;
            var repo = new Mock<IEntityRepository<ParentEntity>>();
            repo.Setup(m => m.Get(item.Id)).Returns(rItem);

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result = manager.Update(item, p => p.FirstName);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("Item could not be found.", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.Get(item.Id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Update_PartialUpdateRepositoryThrowsException_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IEntityRepository<ParentEntity>>();

            repo.Setup(m => m.Get(item.Id)).Returns(item);
            repo.Setup(m => m.Update(item, p => p.FirstName)).Throws(new DataException("Error"));

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = manager.Update(item, p => p.FirstName);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was an issue processing the request, please try again", result.ValidationResults.ElementAt(0).Message);
            Assert.AreEqual("Error", result.ValidationResults.ElementAt(0).Key);
            repo.Verify(m => m.Get(item.Id), Times.Once());
            repo.Verify(m => m.Update(item, p => p.FirstName), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.Error(It.IsAny<DataException>()), Times.Once());
        }

        #endregion
    }
}
