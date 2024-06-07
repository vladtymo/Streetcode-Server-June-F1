namespace Streetcode.XUnitTest.MediatRTests.Streetcode.RelatedFigure.GetByStreetcodeId;

using Moq;
using Xunit;
using AutoMapper;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using AutoMapper.Internal;
using System;
using System.Linq;
using global::Streetcode.DAL.Repositories.Interfaces.Base;
using global::Streetcode.BLL.Interfaces.Logging;
using global::Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;
using global::Streetcode.DAL.Repositories.Interfaces.Streetcode;
using global::Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using global::Streetcode.DAL.Entities.Streetcode;
using global::Streetcode.DAL.Enums;
public class GetRelatedFiguresByStreetcodeIdHandlerTests
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepositoryWrapper> repositoryWrapperMock;
    private readonly Mock<IRelatedFigureRepository> relatedFigureRepositoryMock = new Mock<IRelatedFigureRepository>();
    private readonly Mock<ILoggerService> loggerMock;
    private readonly GetRelatedFiguresByStreetcodeIdHandler handler;

    public GetRelatedFiguresByStreetcodeIdHandlerTests()
    {
        this.mapperMock = new Mock<IMapper>();
        this.repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        this.loggerMock = new Mock<ILoggerService>();
        this.handler = new GetRelatedFiguresByStreetcodeIdHandler(this.mapperMock.Object, this.repositoryWrapperMock.Object, this.loggerMock.Object);
    }

    [Fact]
    public void GetRelatedFigureIdsByStreetcodeId_ShouldReturnNull_WhenArgumentNullExceptionIsThrown()
    {
        var streetcodeId = 1;
        this.relatedFigureRepositoryMock.Setup(r => r.FindAll(It.IsAny<Expression<Func<RelatedFigure, bool>>>()))
                                      .Throws<ArgumentNullException>();

        this.repositoryWrapperMock.Setup(r => r.RelatedFigureRepository).Returns(this.relatedFigureRepositoryMock.Object);

        var method = this.handler.GetType().GetMethod("GetRelatedFigureIdsByStreetcodeId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var result = method?.Invoke(this.handler, new object[] { streetcodeId });

        Assert.Null(result);
    }

    [Fact]
    public void GetRelatedFigureIdsByStreetcodeId_ShouldReturnIQueryable_WhenIdsAreFound()
    {
        var streetcodeId = 1;
        var observerIds = new List<int> { 1, 2, 3, 5 };
        var targetIds = new List<int> { 4, 5, 6, 1 };
        var expectedRes = observerIds.Concat(targetIds).AsQueryable().Distinct();

        this.relatedFigureRepositoryMock.SetupSequence(r => r.FindAll(It.IsAny<Expression<Func<RelatedFigure, bool>>>()))
                                   .Returns(observerIds.Select(id => new RelatedFigure() { ObserverId = id }).Select(id => id).AsQueryable())
                                   .Returns(targetIds.Select(id => new RelatedFigure() { TargetId = id }).Select(id => id).AsQueryable());

        this.repositoryWrapperMock.Setup(r => r.RelatedFigureRepository).Returns(this.relatedFigureRepositoryMock.Object);

        var method = this.handler.GetType().GetMethod("GetRelatedFigureIdsByStreetcodeId", BindingFlags.NonPublic | BindingFlags.Instance);
        var result = method?.Invoke(this.handler, new object[] { streetcodeId }) as IQueryable<int>;

        Assert.NotNull(result);
        Assert.Equal(6, result.Count());
        Assert.Equal(expectedRes, result);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenRelatedFiguresAreFound()
    {
        var streetcodeId = 1;
        var relatedFigures = new List<StreetcodeContent>
        {
            new StreetcodeContent { Id = 1, Status = StreetcodeStatus.Published, Images = new List<DAL.Entities.Media.Images.Image>() },
            new StreetcodeContent { Id = 2, Status = StreetcodeStatus.Published, Images = new List<DAL.Entities.Media.Images.Image>() },
            new StreetcodeContent { Id = 3, Status = StreetcodeStatus.Published, Images = new List<DAL.Entities.Media.Images.Image>() },
        };

        this.mapperMock.Setup(m => m.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
                  .Returns(It.IsAny<List<RelatedFigureDTO>>());

        this.repositoryWrapperMock.Setup(r => r.RelatedFigureRepository.FindAll(It.IsAny<Expression<Func<RelatedFigure, bool>>>()))
                             .Returns(new List<RelatedFigure>
                             {
                                 new RelatedFigure { ObserverId = 1, TargetId = streetcodeId },
                                 new RelatedFigure { ObserverId = 2, TargetId = streetcodeId },
                                 new RelatedFigure { ObserverId = 3, TargetId = streetcodeId },
                             }.AsQueryable());

        this.repositoryWrapperMock.Setup(r => r.StreetcodeRepository
        .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                              .ReturnsAsync(relatedFigures.AsQueryable());

        var result = await this.handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailResult_WhenRelatedFigureIdsNotFound()
    {
        var streetcodeId = 1;
        var request = new GetRelatedFigureByStreetcodeIdQuery(streetcodeId);

        this.mapperMock.Setup(m => m.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
                  .Returns(It.IsAny<List<RelatedFigureDTO>>());

        this.repositoryWrapperMock.Setup(r => r.RelatedFigureRepository.FindAll(It.IsAny<Expression<Func<RelatedFigure, bool>>>()))
            .Returns(Enumerable.Empty<RelatedFigure>().AsQueryable());

        this.repositoryWrapperMock.Setup(r => r.StreetcodeRepository
        .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                              .ReturnsAsync(It.IsAny<List<StreetcodeContent>>);

        var result = await this.handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        Assert.False(result.IsSuccess);
        var expectedErrorMsg = $"Cannot find any related figures by a streetcode id: {streetcodeId}";

        this.loggerMock.Verify(
        logger => logger.LogError(request, expectedErrorMsg),
        Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailResult_WhenRelatedFiguresNotFound()
    {
        var streetcodeId = 1;
        var request = new GetRelatedFigureByStreetcodeIdQuery(streetcodeId);
        var expectedErrorMsg = $"Cannot find any related figures by a streetcode id: {streetcodeId}";

        this.mapperMock.Setup(m => m.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
                  .Returns(new List<RelatedFigureDTO>());

        this.repositoryWrapperMock.Setup(r => r.RelatedFigureRepository.FindAll(It.IsAny<Expression<Func<RelatedFigure, bool>>>()))
            .Returns(It.IsAny<IQueryable<RelatedFigure>>);

        this.repositoryWrapperMock.Setup(r => r.StreetcodeRepository
        .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                              .ReturnsAsync(Enumerable.Empty<StreetcodeContent>().AsQueryable());

        var result = await this.handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);

        this.loggerMock.Verify(
        logger => logger.LogError(request, expectedErrorMsg),
        Times.Once);
    }
}