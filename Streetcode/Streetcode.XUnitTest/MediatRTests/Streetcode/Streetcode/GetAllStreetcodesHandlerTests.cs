using Ardalis.Specification;
using AutoMapper;
using FluentResults;
using Moq;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;
using Streetcode.BLL.Specification.Streetcode.Streetcode.GetAll;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetcodeTests.StreetcodeTest;

public class GetAllStreetcodesHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly GetAllStreetcodesHandler _handler;

    public GetAllStreetcodesHandlerTests()
    {
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerService>();
        _handler = new GetAllStreetcodesHandler(_repositoryWrapperMock.Object, _mapperMock.Object, _loggerMock.Object);
    }


    [Fact]
    public async Task Handle_ShouldReturnFailResult_WhenAmountOrPageIsZeroOrNegative()
    {
        // Arrange
       IError msgError = new Error("Amount and page must be greater than zero");
       var query = CreateQuery(-1, 1);

        // Act
       var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
       Assert.True(result.Errors?.Exists(e => e.Message == msgError.Message));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailResult_WhenReturnNull()
    {
        // Arrange
        var query = CreateQuery(10, 1);
        IError msgError = new Error("The streetcode content is null");
        _repositoryWrapperMock
            .Setup(x => x.StreetcodeRepository.GetAllWithSpecAsync(It.IsAny<ISpecification<StreetcodeContent>[]>()))
            .ReturnsAsync((IEnumerable<StreetcodeContent>)null!);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.Errors?.Exists(e => e.Message == msgError.Message));
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenEntitiesFound()
    {
        // Arrange
        var query = CreateQuery(10, 1);
        var streetcodes = GetSampleStreetcodes();
        var streetcodeDtos = GetSampleStreetcodeDTOs();

        _repositoryWrapperMock
            .Setup(x => x.StreetcodeRepository.GetAllWithSpecAsync(It.IsAny<ISpecification<StreetcodeContent>[]>()))
            .ReturnsAsync(streetcodes);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<StreetcodeDTO>>(streetcodes))
            .Returns(streetcodeDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(
        () => Assert.True(result.IsSuccess),
        () => Assert.Equal(3, result.Value?.Streetcodes?.Count()),
        () => Assert.Equal(1, result.Value?.Pages));
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenMatchByTitle()
    {
        // Arrange
        var title = "Streetcode 1";
        var query = CreateQuery(10, 1, title);
        var streetcode = new StreetcodeContent { Id = 1, Title = title, Status = StreetcodeStatus.Published };
        var streetcodeDto = new StreetcodeDTO { Id = 1, Title = title, Status = StreetcodeStatus.Published };

        _repositoryWrapperMock
            .Setup(x => x.StreetcodeRepository.GetAllWithSpecAsync(It.Is<ISpecification<StreetcodeContent>>(spec => spec is StreetcodesFindWithMatchTitleSpec)))
            .ReturnsAsync(new List<StreetcodeContent> { streetcode });

        _mapperMock
            .Setup(m => m.Map<IEnumerable<StreetcodeDTO>>(It.IsAny<IEnumerable<StreetcodeContent>>()))
            .Returns(new List<StreetcodeDTO> { streetcodeDto });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(1, result.Value.Streetcodes?.Count()),
            () => Assert.Equal(title, result.Value.Streetcodes?.First().Title)
        );
    }

    private GetAllStreetcodesQuery CreateQuery(int amount, int page, string? title = null, string? sort = null, string? filter = null)
    {
        return new GetAllStreetcodesQuery(new GetAllStreetcodesRequestDTO
        {
            Amount = amount,
            Page = page,
            Title = title,
            Sort = sort,
            Filter = filter
        });
    }

    private List<StreetcodeContent> GetSampleStreetcodes()
    {
        return new List<StreetcodeContent>
            {
                new StreetcodeContent { Id = 1, Title = "Streetcode 1", Status = StreetcodeStatus.Published },
                new StreetcodeContent { Id = 2, Title = "Streetcode 2", Status = StreetcodeStatus.Published },
                new StreetcodeContent { Id = 3, Title = "Streetcode 3", Status = StreetcodeStatus.Deleted }
            };
    }

    private List<StreetcodeDTO> GetSampleStreetcodeDTOs()
    {
        return new List<StreetcodeDTO>
            {
                new StreetcodeDTO { Id = 1, Title = "Streetcode 1", Status = StreetcodeStatus.Published },
                new StreetcodeDTO { Id = 2, Title = "Streetcode 2", Status = StreetcodeStatus.Published },
                new StreetcodeDTO { Id = 3, Title = "Streetcode 3", Status = StreetcodeStatus.Deleted }
            };
    }
}