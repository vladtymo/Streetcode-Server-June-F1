// <copyright file="GetFactByStreetcodeIdHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Streetcode.XUnitTest.MediatRTests.StreetcodeTests.Facts
{
    using System.Linq.Expressions;
    using AutoMapper;
    using Moq;
    using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
    using Streetcode.BLL.Interfaces.Logging;
    using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
    using Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;
    using Streetcode.DAL.Entities.Streetcode.TextContent;
    using Streetcode.DAL.Repositories.Interfaces.Base;
    using Xunit;

    public class GetFactByStreetcodeIdHandlerTests
    {
        private const string ERRORMESSAGE = "Cannot find any fact by the streetcode id: ";

        private readonly Mock<IRepositoryWrapper> mockRepositoryWrapper;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly List<Fact> facts;
        private readonly List<FactDto> mappedFacts;

        public GetFactByStreetcodeIdHandlerTests()
        {
            this.mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.facts = new List<Fact> { new Fact { Id = 1, Title = "Test Title", FactContent = "Test Content" } };
            this.mappedFacts = new List<FactDto>() { new FactDto { Id = 1, Title = "Test Title", FactContent = "Test Content" } };
        }

        [Fact]
        public async Task Handle_Success_WhenRepositoryReturnsList()
        {
            // Arrange
            this.mockRepositoryWrapper.
                Setup(repo => repo.FactRepository.GetAllAsync(default, default)).ReturnsAsync(this.facts);
            var handler = new GetFactByStreetcodeIdHandler(
                this.mockRepositoryWrapper.Object,
                this.mockMapper.Object,
                this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetFactByStreetcodeIdQuery(this.facts[0].Id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_ReturnsMappedFacts_WhenRepositoryReturnsData()
        {
            // Arrange
            this.mockRepositoryWrapper.
                 Setup(repo => repo.FactRepository.GetAllAsync(default, default)).ReturnsAsync(this.facts);

            this.mockMapper.Setup(mapper => mapper.Map<IEnumerable<FactDto>>(It.IsAny<IEnumerable<Fact>>()))
                .Returns(this.mappedFacts);
            var handler = new GetFactByStreetcodeIdHandler(
                this.mockRepositoryWrapper.Object,
                this.mockMapper.Object,
                this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetFactByStreetcodeIdQuery(this.facts[0].Id), CancellationToken.None);

            // Assert
            Assert.Equal(this.mappedFacts[0].Id, result.Value.ToArray()[0].Id);
        }

        [Fact]
        public async Task Handle_ReturnError_WhenRepositoryReturnsNull()
        {
            // Arrange
            this.mockRepositoryWrapper
                .Setup(repo => repo.FactRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Fact, bool>>>(), default))
                .ReturnsAsync((IEnumerable<Fact>)null!);

            var handler = new GetFactByStreetcodeIdHandler(
                this.mockRepositoryWrapper.Object,
                this.mockMapper.Object,
                this.mockLogger.Object);

            // Act
            var result = await handler.Handle(
                new GetFactByStreetcodeIdQuery(this.facts[0].Id),
                CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task Handle_ReturnErrorMessage_WhenRepositoryReturnsNull()
        {
            // Arrange
            this.mockRepositoryWrapper
                .Setup(repo => repo.FactRepository.GetAllAsync(
                    It.IsAny<Expression<Func<Fact, bool>>>(), default))
                .ReturnsAsync((IEnumerable<Fact>)null!);

            var handler = new GetFactByStreetcodeIdHandler(
                this.mockRepositoryWrapper.Object,
                this.mockMapper.Object,
                this.mockLogger.Object);

            // Act
            var result = await handler.Handle(
                new GetFactByStreetcodeIdQuery(this.facts[0].Id),
                CancellationToken.None);

            // Assert
            Assert.Equal($"{ERRORMESSAGE}{this.facts[0].Id}", result.Errors.FirstOrDefault()?.Message);
        }
    }
}
