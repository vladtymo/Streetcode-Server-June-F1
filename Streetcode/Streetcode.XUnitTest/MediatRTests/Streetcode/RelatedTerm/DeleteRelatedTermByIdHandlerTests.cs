namespace Streetcode.XUnitTest.MediatRTests.StreetcodeTests.RelatedTerm;

using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;

using AutoMapper;
using Moq;
using Xunit;

using Streetcode.BLL.DTO.Streetcode.TextContent.RelatedTerm;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.DeleteById;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;
using Streetcode.BLL.Resources;

public class DeleteRelatedTermByIdHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly DeleteRelatedTermByIdHandler _handler;

    public DeleteRelatedTermByIdHandlerTests()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _handler = new DeleteRelatedTermByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenRelatedTermNotFound()
    {
        // Arrange
        var request = new DeleteRelatedTermByIdCommand(1);
        _mockRepository.Setup(x => x.RelatedTermRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entity, bool>>>(), null))
            .ReturnsAsync((Entity?)null);
        var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.EntityWithIdNotFound, request);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal(errorMsg, result.Errors.First().Message);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenRelatedTermDeleted()
    {
        // Arrange
        var relatedTerm = new Entity { Id = 1, Word = "Test" };
        var relatedTermDto = new RelatedTermDTO { Id = 1, Word = "Test" };

        _mockRepository.Setup(x => x.RelatedTermRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entity, bool>>>(), null))
            .ReturnsAsync(relatedTerm);
        _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        _mockMapper.Setup(mapper => mapper.Map<RelatedTermDTO>(relatedTerm)).Returns(relatedTermDto);

        // Act
        var result = await _handler.Handle(new DeleteRelatedTermByIdCommand(1), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(relatedTermDto, result.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenSaveChangesFails()
    {
        // Arrange
        var relatedTerm = new Entity { Id = 1, Word = "Test" };
        var relatedTermDto = new RelatedTermDTO { Id = 1, Word = "Test" };
        var request = new DeleteRelatedTermByIdCommand(1);

        _mockRepository.Setup(x => x.RelatedTermRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entity, bool>>>(), null))
            .ReturnsAsync(relatedTerm);
        _mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(0);
        _mockMapper.Setup(mapper => mapper.Map<RelatedTermDTO>(relatedTerm)).Returns(relatedTermDto);
        var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.FailToDeleteA, request);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal(errorMsg, result.Errors.First().Message);
    }
}
