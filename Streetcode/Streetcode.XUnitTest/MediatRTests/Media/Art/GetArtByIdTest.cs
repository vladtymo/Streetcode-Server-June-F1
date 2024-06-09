namespace Streetcode.XUnitTest.MediatRTests.Media.Art;
using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Art.GetById;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
public class GetArtByIdTest
{
    private readonly Mock<IRepositoryWrapper> repositoryWrapperMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<ILoggerService> loggerMock;
    private readonly GetArtByIdHandler handler;

    public GetArtByIdTest()
    {
        this.repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        this.mapperMock = new Mock<IMapper>();
        this.loggerMock = new Mock<ILoggerService>();
        this.handler = new GetArtByIdHandler(this.repositoryWrapperMock.Object, this.mapperMock.Object, this.loggerMock.Object);
    }

    [Fact]
    public async Task Test_Handle_ArtExists_ReturnsSuccessResult()
    {
        // Arrange
        var artId = 1;
        var request = new GetArtByIdQuery(artId);
        var artEntity = new DAL.Entities.Media.Images.Art { Id = artId };

        this.repositoryWrapperMock.Setup(repo => repo.ArtRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
            .ReturnsAsync(artEntity);

        // Act
        var result = await this.handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Test_Handle_ArtDoesNotExist_ReturnsErrorResult_IsSuccessIsFalse()
    {
        // Arrange
        var artId = 1;
        var request = new GetArtByIdQuery(artId);

        this.repositoryWrapperMock.Setup(repo => repo.ArtRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
            .ReturnsAsync(null as DAL.Entities.Media.Images.Art);

        // Act
        var result = await this.handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }
}