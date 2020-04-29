using FluentAssertions;
using Moq;
using Plank.Net.Data;
using Plank.Net.Managers;
using Plank.Net.Tests.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;
using Xunit;

namespace Plank.Net.Tests.Managers
{
    public class PlankManagerTests
    {
        #region MEMBERS

        private readonly Mock<ILogger> _logger;

        #endregion

        #region CONSTRUCTORS

        public PlankManagerTests()
        {
            _logger = new Mock<ILogger>();
        }

        #endregion

        #region METHODS

        [Fact]
        public void Constructor_RepositoryNull_ArgumentNullException()
        {
            // Arrange

            // Act
            Action act = () => CreateManagerNullRepository();

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Value cannot be null.\r\nParameter name: repository");

            void CreateManagerNullRepository()
            {
                var manager = new PlankManager<ParentEntity>(null, _logger.Object);
            }
        }

        [Fact]
        public void Constructor_LoggerNull_ArgumentNullException()
        {
            // Arrange
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            Action act = () => CreateManagerNullLogger();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: logger");

            void CreateManagerNullLogger()
            {
                var manager = new PlankManager<ParentEntity>(repo.Object, null);
            }
        }

        [Fact]
        public async Task Create_EntityValid_Created()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.AddAsync(item)).Returns(Task.FromResult(item));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.AddAsync(item);

            // Assert
            result.ValidationResults.IsValid.Should().BeTrue();
            result.Item.Id.Should().Be(item.Id);
            repo.Verify(m => m.AddAsync(item), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Create_EntityNotValid_NotCreated()
        {
            // Arrange
            var item       = TestHelper.GetParentEntity();
            item.FirstName = string.Empty;

            var repo       = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await  manager.AddAsync(item);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            repo.Verify(m => m.AddAsync(item), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
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

            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.AddAsync(item);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            repo.Verify(m => m.AddAsync(item), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Create_EntityNull_NotCreated()
        {
            // Arrange
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.AddAsync(null);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("ParentEntity cannot be null.");
            repo.Verify(m => m.AddAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Create_FluentValidatorHasPassResult_Created()
        {
            // Arrange
            var entity = new ChildThree
            {
                Id = 1
            };
            var repo = new Mock<IRepository<ChildThree>>();
            var logger = new Mock<ILogger>();

            // Act
            var manager = new PlankManager<ChildThree>(repo.Object, logger.Object);
            var result = await manager.AddAsync(entity);

            // Assert
            result.ValidationResults.IsValid.Should().BeTrue();
            repo.Verify(m => m.AddAsync(It.IsAny<ChildThree>()), Times.Once());
            logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Create_FluentValidatorHasFailResult_NotCreated()
        {
            // Arrange
            var entity = new ChildThree();
            var repo = new Mock<IRepository<ChildThree>>();
            var logger = new Mock<ILogger>();

            // Act
            var manager = new PlankManager<ChildThree>(repo.Object, logger.Object);
            var result = await manager.AddAsync(entity);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("'Id' must be greater than '0'.");
            repo.Verify(m => m.AddAsync(It.IsAny<ChildThree>()), Times.Never);
            logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Create_ValidatorHasFailResult_NotCreated()
        {
            // Arrange
            var entity = new GrandParentEntity();
            var repo   = new Mock<IRepository<GrandParentEntity>>();
            var logger = new Mock<ILogger>();

            // Act
            var manager = new PlankManager<GrandParentEntity>(repo.Object, logger.Object);
            var result = await manager.AddAsync(entity);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("There was a problem");
            repo.Verify(m => m.AddAsync(It.IsAny<GrandParentEntity>()), Times.Never());
            logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Create_ValidatorHasFailResultOnChildEntity_NotCreated()
        {
            // Arrange
            var entity = TestHelper.GetParentEntity();
            entity.ChildOne = new List<ChildOne> { TestHelper.GetChildOne() };
            entity.ChildTwo = new List<ChildTwo> { TestHelper.GetChildTwo() };
            var repo   = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.AddAsync(entity);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("There was a problem");
            repo.Verify(m => m.AddAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Create_RepositoryThrowException_NotCreated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.AddAsync(item)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.AddAsync(item);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            result.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once);
        }

        [Fact]
        public async Task Delete_EntityExists_Deleted()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var id   = 1;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.DeleteAsync(id)).Returns(Task.FromResult(id));
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var results = await manager.DeleteAsync(id);

            // Assert
            results.ValidationResults.IsValid.Should().BeTrue();
            results.Id.Should().Be(id);
            repo.Verify(m => m.GetAsync(id), Times.Once());
            repo.Verify(m => m.DeleteAsync(id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Delete_EntityNotFound_NotDeleted()
        {
            // Arrange
            ParentEntity item = null;
            var id   = 1;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var results = await manager.DeleteAsync(id);

            // Assert
            results.ValidationResults.IsValid.Should().BeTrue();
            repo.Verify(m => m.GetAsync(id), Times.Once());
            repo.Verify(m => m.DeleteAsync(id), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Delete_RepositoryThrowsException_NotDeleted()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var id   = 1;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.DeleteAsync(id)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var results = await manager.DeleteAsync(id);

            // Assert
            results.ValidationResults.IsValid.Should().BeFalse();
            results.ValidationResults.ElementAt(0).Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            results.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(id), Times.Once());
            repo.Verify(m => m.DeleteAsync(id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());
        }

        [Fact]
        public async Task Get_EntityFoundById_ItemReturned()
        {
            // Arrange
            var id   = 1;
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Returns(Task.FromResult(item));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.GetAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            repo.Verify(m => m.GetAsync(id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Get_RepositoryThrowsException_IsValidFalse()
        {
            // Arrange
            var id = 1;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(id)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.GetAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            repo.Verify(m => m.GetAsync(id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<int>()), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Once());
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());
        }

        [Fact]
        public async Task Search_NoCriteria_PageReturned()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var item = TestHelper.GetParentEntity();
            var list = new List<ParentEntity>() { item, item }.ToPagedList(1, 10);
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), pageNumber, pageSize)).Returns(Task.FromResult(list));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.SearchAsync(null, pageNumber, pageSize);

            // Assert
            result.IsValid.Should().BeTrue();
            result.IsFirstPage.Should().BeTrue();
            result.IsLastPage.Should().BeTrue();
            result.HasNextPage.Should().BeFalse();
            result.HasPreviousPage.Should().BeFalse();
            result.Items.Should().HaveCount(2);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalItemCount.Should().Be(2);
            repo.Verify(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), 1, 10), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Search_RepositoryThrowsException_IsValidFalse()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize   = 10;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), pageNumber, pageSize)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.SearchAsync(null, pageNumber, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            repo.Verify(m => m.SearchAsync(It.IsAny<Expression<Func<ParentEntity, bool>>>(), pageNumber, pageSize), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());

        }

        [Fact]
        public async Task Update_EntityValid_Updated()
        {
            // Arrange
            var existing = TestHelper.GetParentEntity();
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(existing));
            repo.Setup(m => m.UpdateAsync(existing)).Returns(Task.FromResult(existing));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            result.ValidationResults.IsValid.Should().BeTrue();
            result.Item.Id.Should().Be(item.Id);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(existing), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_EntityNotValid_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            item.FirstName = null;

            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            repo.Verify(m => m.UpdateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_EntityNull_NotUpdated()
        {
            // Arrange
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(null);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("ParentEntity cannot be null.");
            repo.Verify(m => m.UpdateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_EntityNotFound_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            ParentEntity rItem = null;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(rItem));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("Item could not be found");
            result.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_ValidatorHasFailResult_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            item.ChildOne = new List<ChildOne> { TestHelper.GetChildOne() };
            item.ChildTwo = new List<ChildTwo> { TestHelper.GetChildTwo() };
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("There was a problem");
            repo.Verify(m => m.UpdateAsync(It.IsAny<ParentEntity>()), Times.Never());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_RepositoryThrowsException_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();

            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.UpdateAsync(item)).Throws(new DataException("Error"));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            result.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(item), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());
        }

        [Fact]
        public async Task Update_PartialUpdateEntityValid_Updated()
        {
            // Arrange
            var existing = TestHelper.GetParentEntity();
            var item = new ParentEntity { Id = 0, IsActive = false };
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(existing));
            repo.Setup(m => m.UpdateAsync(existing)).Returns(Task.FromResult(existing));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.UpdateAsync(item, p => p.IsActive);

            // Assert
            result.ValidationResults.IsValid.Should().BeTrue();
            result.Item.Id.Should().Be(item.Id);
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(existing), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));

        }

        [Fact]
        public async Task Update_PartialUpdateEntityNull_NotUpdated()
        {
            // Arrange
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(null, p => p.FirstName);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("Value cannot be null or empty.\r\nParameter name: item");
            result.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdatePropertiesNull_NotUpdated()
        {
            // Arrange
            var existing = TestHelper.GetParentEntity();
            var item = new ParentEntity { Id = 0, IsActive = false };
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item, null);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("Value cannot be null or empty.\r\nParameter name: properties");
            result.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdateInvalidProperty_NotUpdated()
        {
            // Arrange
            var existing = TestHelper.GetParentEntity();
            var item = new ParentEntity { Id = 0, IsActive = false };
            var repo = new Mock<IRepository<ParentEntity>>();

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item, p => p, p => p.IsActive);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("Value cannot be null or empty.\r\nParameter name: properties");
            result.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdateEntityNotFound_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            ParentEntity rItem = null;
            var repo = new Mock<IRepository<ParentEntity>>();
            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(rItem));

            // Act
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result = await manager.UpdateAsync(item, p => p.FirstName);

            // Assert
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("Item could not be found");
            result.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Update_PartialUpdateRepositoryThrowsException_NotUpdated()
        {
            // Arrange
            var item = TestHelper.GetParentEntity();
            var repo = new Mock<IRepository<ParentEntity>>();

            repo.Setup(m => m.GetAsync(item.Id)).Returns(Task.FromResult(item));
            repo.Setup(m => m.UpdateAsync(item)).Throws(new DataException("Error"));

            // Act
            //
            var manager = new PlankManager<ParentEntity>(repo.Object, _logger.Object);
            var result  = await manager.UpdateAsync(item, p => p.FirstName);

            // Assert
            //
            result.ValidationResults.IsValid.Should().BeFalse();
            result.ValidationResults.ElementAt(0).Message.Should().Be("There was an issue processing the request, see the plank logs for details");
            result.ValidationResults.ElementAt(0).Key.Should().Be("Error");
            repo.Verify(m => m.GetAsync(item.Id), Times.Once());
            repo.Verify(m => m.UpdateAsync(item), Times.Once());
            _logger.Verify(m => m.InfoMessage(It.IsAny<string>()), Times.Exactly(2));
            _logger.Verify(m => m.ErrorMessage(It.IsAny<DataException>()), Times.Once());
        }

        #endregion
    }
}
