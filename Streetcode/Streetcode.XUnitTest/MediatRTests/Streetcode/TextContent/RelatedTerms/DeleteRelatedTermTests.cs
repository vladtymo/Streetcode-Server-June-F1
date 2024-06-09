using AutoMapper;
using Moq;
using NLog.Targets;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Streetcode.TextContent.RelatedTerms
{
    public class DeleteRelatedTermCommandHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public DeleteRelatedTermCommandHandlerTests()
        {
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenTermNotFound()
        {
            // Arrange
            string word = "test";
            this.MockRepositorySetup(true);

            var handler = new DeleteRelatedTermHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteRelatedTermCommand(word), CancellationToken.None);

            // Assert
            Assert.True(result.HasError(e => e.Message == $"Cannot find a related term: {word}"));
            this._mockLogger.Verify(x => x.LogError(It.IsAny<object>(), $"Cannot find a related term: {word}"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenTermDeletedSuccessfully()
        {
            // Arrange
            string word = "test";
            this.MockRepositorySetup(false, word);
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            this.MockMapperSetup(false, word);

            var handler = new DeleteRelatedTermHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteRelatedTermCommand(word), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            this._mockRepository.Verify(x => x.RelatedTermRepository.Delete(It.IsAny<RelatedTerm>()), Times.Once);
            this._mockMapper.Verify(x => x.Map<RelatedTermDTO>(It.IsAny<RelatedTerm>()), Times.Once);
            this._mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenSaveChangesFails()
        {
            // Arrange
            string word = "test";
            this.MockRepositorySetup(false, word);
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
            this.MockMapperSetup(false, word);

            var handler = new DeleteRelatedTermHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteRelatedTermCommand(word), CancellationToken.None);

            // Assert
            Assert.True(result.HasError(e => e.Message == "Failed to delete a related term"));
            this._mockLogger.Verify(x => x.LogError(It.IsAny<object>(), "Failed to delete a related term"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenMappingFails()
        {
            // Arrange
            string word = "test";

            this.MockRepositorySetup(false, word);
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            this.MockMapperSetup(true);

            var handler = new DeleteRelatedTermHandler(
            this._mockRepository.Object,
            this._mockMapper.Object,
            this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteRelatedTermCommand(word), CancellationToken.None);

            // Assert
            Assert.True(result.HasError(e => e.Message == "Failed to delete a related term"));
            this._mockLogger.Verify(x => x.LogError(It.IsAny<object>(), "Failed to delete a related term"), Times.Once);
        }

        private void MockRepositorySetup(bool returnNull, string word = "")
        {
            this._mockRepository.Setup(x => x.RelatedTermRepository
                .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<RelatedTerm, bool>>>(), null))
                .ReturnsAsync(returnNull ? (RelatedTerm)null! : new RelatedTerm { Word = word });
        }

        private void MockMapperSetup(bool returnNull, string word = "")
        {
            this._mockMapper.Setup(x => x.Map<RelatedTermDTO>(It.IsAny<RelatedTerm>()))
                .Returns(returnNull ? (RelatedTermDTO)null! : new RelatedTermDTO { Word = word });
        }
    }


}
