using Moq;
using Xunit;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.XUnitTest.MediatRTests.StreetcodeTests.RelatedFigureTests.GetByStreetcodeId;

public class GetRelatedFiguresByStreetcodeIdHandlerTests
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IRepositoryWrapper> repositoryWrapperMock;
    private readonly Mock<ILoggerService> loggerMock;
    private readonly GetRelatedFiguresByStreetcodeIdHandler handler;

    public GetRelatedFiguresByStreetcodeIdHandlerTests()
    {
        mapperMock = new Mock<IMapper>();
        repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        loggerMock = new Mock<ILoggerService>();
        handler = new GetRelatedFiguresByStreetcodeIdHandler(mapperMock.Object, repositoryWrapperMock.Object, loggerMock.Object);

        mapperMock.Setup(m => m.Map<IEnumerable<RelatedFigureDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
          .Returns(It.IsAny<List<RelatedFigureDTO>>());
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

        repositoryWrapperMock.Setup(r => r.RelatedFigureRepository.FindAll(It.IsAny<Expression<Func<RelatedFigure, bool>>>()))
                             .Returns(new List<RelatedFigure>
                             {
                                 new RelatedFigure { ObserverId = 1, TargetId = streetcodeId },
                                 new RelatedFigure { ObserverId = 2, TargetId = streetcodeId },
                                 new RelatedFigure { ObserverId = 3, TargetId = streetcodeId },
                             }.AsQueryable());

        repositoryWrapperMock.Setup(r => r.StreetcodeRepository
        .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                              .ReturnsAsync(relatedFigures.AsQueryable());

        var result = await handler.Handle(new GetRelatedFigureByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailResult_WhenRelatedFiguresNotFound()
    {
        var streetcodeId = 1;
        var request = new GetRelatedFigureByStreetcodeIdQuery(streetcodeId);

        repositoryWrapperMock.Setup(r => r.RelatedFigureRepository.FindAll(It.IsAny<Expression<Func<RelatedFigure, bool>>>()))
            .Returns(Enumerable.Empty<RelatedFigure>().AsQueryable());

        repositoryWrapperMock.Setup(r => r.StreetcodeRepository
        .GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                              .ReturnsAsync(It.IsAny<List<StreetcodeContent>>);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
    }
}