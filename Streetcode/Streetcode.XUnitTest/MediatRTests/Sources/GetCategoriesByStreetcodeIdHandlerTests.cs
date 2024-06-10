namespace Streetcode.XUnitTest.MediatRTests.Sources;

using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

public class GetCategoriesByStreetcodeIdHandlerTests
{
    private readonly Mock<IRepositoryWrapper> repositoryWrapperMock;
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IBlobService> blobServiceMock;
    private readonly Mock<ILoggerService> loggerMock;
    private readonly GetCategoriesByStreetcodeIdHandler handler;
    private readonly List<SourceLinkCategory> listSourceCategories;
    private readonly List<SourceLinkCategoryDTO> listSourceCategoryDTO;

    public GetCategoriesByStreetcodeIdHandlerTests()
    {
        this.repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        this.mapperMock = new Mock<IMapper>();
        this.blobServiceMock = new Mock<IBlobService>();
        this.loggerMock = new Mock<ILoggerService>();
        this.handler = new GetCategoriesByStreetcodeIdHandler(this.repositoryWrapperMock.Object, this.mapperMock.Object, this.blobServiceMock.Object, this.loggerMock.Object);
        this.listSourceCategories = new List<SourceLinkCategory>
        {
            new ()
            {
                Id = 1,
                Title = "Test1",
                ImageId = 1,
                Image = new () { BlobName = "blob1", Base64 = string.Empty },
                Streetcodes = new () { new StreetcodeContent() { Id = 1 }, },
            },
        };

        this.listSourceCategoryDTO = new List<SourceLinkCategoryDTO>
        {
            new ()
            {
                Id = 1,
                Title = "Test1",
                ImageId = 1,
                Image = new ImageDTO { BlobName = "blob1", Base64 = string.Empty },
            },
        };
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenCategoriesAreNull()
    {
        // Arrange
        var request = new GetCategoriesByStreetcodeIdQuery(1);
        IError error = new Error($"Cant find any source category with the streetcode id {request.StreetcodeId}");

        this.repositoryWrapperMock.Setup(repo => repo.SourceCategoryRepository.GetAllAsync(
            It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
            It.IsAny<Func<IQueryable<SourceLinkCategory>, IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync((IEnumerable<SourceLinkCategory>)null!);

        // Act
        var result = await this.handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Errors?.Exists(e => e.Message == error.Message));
    }

    [Fact]
    public async Task Handle_ShouldReturnIEnumarableCategoryDTOs_WhenCategoriesExist()
    {
        // Arrange
        string base64 = "base64-encoded-string-1";

        this.repositoryWrapperMock.Setup(repo => repo.SourceCategoryRepository.GetAllAsync(
             It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
             It.IsAny<Func<IQueryable<SourceLinkCategory>, IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(this.listSourceCategories);

        this.blobServiceMock.Setup(blobService => blobService.FindFileInStorageAsBase64("blob1")).Returns(base64);

        this.mapperMock.Setup(mapper => mapper.Map<IEnumerable<SourceLinkCategoryDTO>>(this.listSourceCategories))
            .Returns(this.listSourceCategoryDTO);

        // Act
        await this.handler.Handle(new GetCategoriesByStreetcodeIdQuery(1), CancellationToken.None);

        // Assert
        Assert.Equal(this.listSourceCategoryDTO[0].Image?.Base64, base64);
    }
}