using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.GetAllByTermId;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Streetcode.RelatedTerms
{
    public class GetAllRelatedTermsByTermIdTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetAllRelatedTermsByTermIdTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task GetAllRelatedTermsByTermId_ShouldReturnError_IfRelatedTermsIsNull()
        {
            // Arrange
            int id = 1;
            MockRepositorySetup(true);

            var handler = new GetAllRelatedTermsByTermIdHandler(
                _mockMapper.Object,
                _mockRepository.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllRelatedTermsByTermIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.HasError(e => e.Message == "Cannot get words by term id"));
        }

        [Fact]
        public async Task GetAllRelatedTermsByTermId_ShouldReturnError_IfRelatedTermsDTOIsNull()
        {
            // Arrange
            int id = 1;
            MockRepositorySetup(false);
            MockMapperSetup(true);

            var handler = new GetAllRelatedTermsByTermIdHandler(
                _mockMapper.Object,
                _mockRepository.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllRelatedTermsByTermIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.HasError(e => e.Message == "Cannot create DTOs for related words!"));
        }

        [Fact]
        public async Task GetAllRelatedTermsByTermId_ShouldReturnOk_WhenArgumentsPassedCorrectly()
        {
            // Arrange
            int id = 1;
            MockRepositorySetup(false);
            MockMapperSetup(false);

            var handler = new GetAllRelatedTermsByTermIdHandler(
                _mockMapper.Object,
                _mockRepository.Object,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllRelatedTermsByTermIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetAllRelatedTermsByTermId_MapperShouldCallMapOnlyOnce()
        {
            // Arrange
            int id = 1;
            MockRepositorySetup(false);
            MockMapperSetup(false);

            var handler = new GetAllRelatedTermsByTermIdHandler(
                _mockMapper.Object,
                _mockRepository.Object,
                _mockLogger.Object);

            // Act
            await handler.Handle(new GetAllRelatedTermsByTermIdQuery(id), CancellationToken.None);

            // Assert
            _mockMapper.Verify(
                m => m.Map<IEnumerable<RelatedTermDTO>>(It.IsAny<IEnumerable<RelatedTerm>>()),
                Times.Once);
        }

        private static IEnumerable<RelatedTermDTO> GetRelatedTermDTOs()
        {
            return new List<RelatedTermDTO> { };
        }

        private static IEnumerable<RelatedTerm> GetRelatedTerms()
        {
            return new List<RelatedTerm> { };
        }

        private void MockMapperSetup(bool returnNull)
        {
            _mockMapper
                .Setup(x => x
                .Map<IEnumerable<RelatedTermDTO>>(It.IsAny<IEnumerable<RelatedTerm>>()))
                .Returns(returnNull ? null! : GetRelatedTermDTOs());
        }

        private void MockRepositorySetup(bool returnNull)
        {
            _mockRepository.Setup(x => x.RelatedTermRepository
                .GetAllAsync(
                   It.IsAny<Expression<Func<RelatedTerm, bool>>>(),
                   It.IsAny<Func<IQueryable<RelatedTerm>, IIncludableQueryable<RelatedTerm, object>>>()))
                .ReturnsAsync(returnNull ? null! : GetRelatedTerms());
        }
    }

}
