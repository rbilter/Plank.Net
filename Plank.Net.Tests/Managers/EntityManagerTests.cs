using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plank.Net.Data;
using Plank.Net.Managers;
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
            var validators = new List<IValidator<ParentEntity>>();

            // Act
            try
            {
                var manager = new EntityManager<ParentEntity>(null, validators, _logger.Object);
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
        public void Constructor_ValidatorsNull_ArgumentNullException()
        {
            // Arrange
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            try
            {
                var manager = new EntityManager<ParentEntity>(repo.Object, null, _logger.Object);
                Assert.Fail("ArgumentNullException should have been thrown for validators.");
            }
            // Assert
            catch (ArgumentNullException e)
            {
                var msg = "Value cannot be null.\r\nParameter name: validators";
                Assert.AreEqual(msg, e.Message);
            }
        }

        [TestMethod]
        public void Constructor_LoggerNull_ArgumentNullException()
        {
            // Arrange
            var repo       = new Mock<IRepository<ParentEntity>>();
            var validators = new List<IValidator<ParentEntity>>();

            // Act
            try
            {
                var manager = new EntityManager<ParentEntity>(repo.Object, validators, null);
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
            var item       = TestHelper.GetParentEntity();
            var validators = new List<IValidator<ParentEntity>>();
            var repo       = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m   => m.Create(item)).Returns(item.Id);

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
            var result = manager.Create(item);

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

            var repo       = new Mock<IRepository<ParentEntity>>();
            var validators = new List<IValidator<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var invalidChild     = TestHelper.GetChildEntity();
            invalidChild.Address = string.Empty;

            var item = TestHelper.GetParentEntity();
            item.ChildEntities = new List<ChildEntity>
            {
                TestHelper.GetChildEntity(),
                invalidChild
            };

            var validators = new List<IValidator<ParentEntity>>();
            var repo       = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var repo       = new Mock<IRepository<ParentEntity>>();
            var validators = new List<IValidator<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var contact    = TestHelper.GetParentEntity();
            var repo       = new Mock<IRepository<ParentEntity>>();
            var validators = new List<IValidator<ParentEntity>>();
            var validate1  = TestHelper.GetPassValidator();
            var validate2  = TestHelper.GetFailValidator();
            validators.AddRange(new[] { validate1.Object, validate2.Object });

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
            var result = manager.Create(contact);

            // Assert
            //
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was a problem", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.Create(It.IsAny<ParentEntity>()), Times.Never());
            validate1.Verify(m => m.Validate(It.IsAny<ParentEntity>()), Times.Once());
            validate2.Verify(m => m.Validate(It.IsAny<ParentEntity>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Create_RepositoryCreateThrowException_NotCreated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.Create(item)).Throws(new DataException("Error"));

            var validators = new List<IValidator<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.Delete(id)).Returns(id);
            repo.Setup(m => m.Get(id)).Returns(item);

            var validators = new List<IValidator<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.Get(id)).Returns(item);

            var validators = new List<IValidator<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
            var results = manager.Delete(id);

            // Assert
            Assert.IsTrue(results.ValidationResults.IsValid);
            repo.Verify(m => m.Get(id), Times.Once());
            repo.Verify(m => m.Delete(id), Times.Never());
            _logger.Verify(m => m.Info(It.IsAny<Guid>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Delete_RepositoryDeleteThrowsException_NotDeleted()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var id   = Guid.NewGuid();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.Get(id)).Returns(item);
            repo.Setup(m => m.Delete(id)).Throws(new DataException("Error"));

            var validators = new List<IValidator<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.Get(id)).Returns(item);

            var validators = new List<IValidator<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
            var result  = manager.Get(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            repo.Verify(m => m.Get(id), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<Guid>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void Get_RepositoryGetByIdThrowsException_NullReturned()
        {
            // Arrange
            var id = Guid.NewGuid();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.Get(id)).Throws(new DataException("Error"));

            var validators = new List<IValidator<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.Get(item.Id)).Returns(item);
            repo.Setup(m => m.Update(item)).Returns(item.Id);

            var validators = new List<IValidator<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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

            var repo       = new Mock<IRepository<ParentEntity>>();
            var validators = new List<IValidator<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var repo = new Mock<IRepository<ParentEntity>>();
            var validators = new List<IValidator<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.Get(item.Id)).Returns(rItem);

            var validators = new List<IValidator<ParentEntity>>();

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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
            var repo = new Mock<IRepository<ParentEntity>>();

            var validators = new List<IValidator<ParentEntity>>();
            var validate1 = TestHelper.GetPassValidator();
            var validate2 = TestHelper.GetFailValidator();
            validators.AddRange(new[] { validate1.Object, validate2.Object });

            // Act
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
            var result = manager.Update(item);

            // Assert
            Assert.IsFalse(result.ValidationResults.IsValid);
            Assert.AreEqual("There was a problem", result.ValidationResults.ElementAt(0).Message);
            repo.Verify(m => m.Update(It.IsAny<ParentEntity>()), Times.Never());
            validate1.Verify(m => m.Validate(It.IsAny<ParentEntity>()), Times.Once());
            validate2.Verify(m => m.Validate(It.IsAny<ParentEntity>()), Times.Once());
            _logger.Verify(m => m.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Update_RepositoryUpdateThrowsException_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();

            repo.Setup(m => m.Get(item.Id)).Returns(item);
            repo.Setup(m => m.Update(item)).Throws(new DataException("Error"));

            var validators = new List<IValidator<ParentEntity>>();

            // Act
            //
            var manager = new EntityManager<ParentEntity>(repo.Object, validators, _logger.Object);
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

        #endregion
    }
}
