using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;
using Streetcode.BLL.MediatR.Streetcode.Facts.Update;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetcodeTests.Facts
{
    public class UpdateFactHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly UpdateFactHandler _handler;

        public UpdateFactHandlerTests()
        {
            _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILoggerService>();
            _handler = new UpdateFactHandler(_repositoryWrapperMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Tt()
        {
            FactUpdateCreateDto factUpdateCreateDto = new FactUpdateCreateDto() { Id = 100 };
            var fact = new Fact() { Id = 100 };

            var request = new UpdateFactCommand(factUpdateCreateDto);
            _mapperMock.Setup(m => m.Map<Fact>(factUpdateCreateDto)).Returns(fact);

            _repositoryWrapperMock.Setup(r => r.FactRepository.Update(fact));
            _repositoryWrapperMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenFactIsUpdated()
        {
            //// Arrange
            //var factDto = new FactUpdateCreateDto { Id = 2, FactContent = "Fact content" };
            //var fact = new Fact { Id = 2, FactContent = "Fact content" };

            //var command = new UpdateFactCommand(factDto);

            //_mapperMock.Setup(m => m.Map<Fact>(factDto)).Returns(fact);
            //_repositoryWrapperMock.Setup(r => r.FactRepository.Create(fact));

            //_repositoryWrapperMock.Setup(r => r.FactRepository.Update(fact));
            //_repositoryWrapperMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            //// Act
            //var result = await _handler.Handle(command, CancellationToken.None);

            //// Assert
            //Assert.True(result.IsSuccess);
            var factDto = new FactUpdateCreateDto { Id = 1, FactContent = "Fact content" };
            var fact = new Fact { Id = 3, FactContent = "Fact content" };
            var existingFact = new Fact { Id = 2, FactContent = "Existing fact content" };

            var command = new UpdateFactCommand(factDto);

            _mapperMock.Setup(m => m.Map<Fact>(factDto)).Returns(fact);

            _repositoryWrapperMock.Setup(r => r.FactRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Fact, bool>>>(), default)).ReturnsAsync(existingFact);

            _repositoryWrapperMock.Setup(r => r.FactRepository.Update(fact));
            _repositoryWrapperMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenFactIsNull()
        {
            // Arrange
            var factDto = new FactUpdateCreateDto { Id = 1, FactContent = "Fact content" };
            var command = new UpdateFactCommand(factDto);

            _mapperMock.Setup(m => m.Map<Fact>(factDto)).Returns((Fact)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Cannot convert null to fact", result.Errors.First().Message);
            _repositoryWrapperMock.Verify(r => r.FactRepository.Update(It.IsAny<Fact>()), Times.Never);
            _repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ReturnFailure_WhenSaveChangesFails()
        {
            // Arrange
            var factDto = new FactUpdateCreateDto { Id = 1, FactContent = "Fact content" };
            var fact = new Fact { Id = 1, FactContent = "Fact content" };

            var command = new UpdateFactCommand(factDto);

            _mapperMock.Setup(m => m.Map<Fact>(factDto)).Returns(fact);
            _repositoryWrapperMock.Setup(r => r.FactRepository.Update(fact));
            _repositoryWrapperMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to update fact", result.Errors.First().Message);
            _repositoryWrapperMock.Verify(r => r.FactRepository.Update(fact), Times.Once);
            _repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
