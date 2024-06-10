// <copyright file="GetFactByStreetcodeIdHandlerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Streetcode.XUnitTest.MediatRTests.StreetcodeTests.Facts
{
    using AutoMapper;
    using Moq;
    using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
    using Streetcode.BLL.Interfaces.Logging;
    using Streetcode.BLL.MediatR.Streetcode.Fact.GetByStreetcodeId;
    using Streetcode.DAL.Entities.Streetcode.TextContent;
    using Streetcode.DAL.Repositories.Interfaces.Base;
    using System.Linq.Expressions;
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
            this.facts = new List<Fact>
            {
                new Fact { Id = 1, Title = "Test Title", FactContent = "Test Content", StreetcodeId = 1 },
                new Fact { Id = 2, Title = "Test Title2", FactContent = "Test Content2", StreetcodeId = 1 },
                new Fact { Id = 3, Title = "Test Title2", FactContent = "Test Content2", StreetcodeId = 2 },
            };
            this.mappedFacts = new List<FactDto>()
            {
                new FactDto { Id = 1, Title = "Test Title", FactContent = "Test Content" },
                new FactDto { Id = 2 },
            };
        }

        [Fact]
        public async Task Handle_Should_Success_WhenRepositoryHasCorrectParameters()
        {
            // Arrange
            Fact fact = this.facts[0];
            Fact fact1 = this.facts[1];
            Fact otherFact = this.facts[2];

            this.mockRepositoryWrapper.
                 Setup(repo => repo.FactRepository.GetAllAsync(default, default)).ReturnsAsync(this.facts);

            this.mockMapper.Setup(mapper => mapper.Map<IEnumerable<FactDto>>(It.IsAny<IEnumerable<Fact>>()))
                .Returns(this.mappedFacts);
            var handler = new GetFactByStreetcodeIdHandler(
                this.mockRepositoryWrapper.Object,
                this.mockMapper.Object,
                this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetFactByStreetcodeIdQuery(fact.StreetcodeId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => this.mockRepositoryWrapper.Verify(repo => repo.FactRepository.GetAllAsync(
                    It.Is<Expression<Func<Fact, bool>>>(predicate => predicate.Compile().Invoke(fact)),
                    default)),
                () => this.mockRepositoryWrapper.Verify(repo => repo.FactRepository.GetAllAsync(
                    It.Is<Expression<Func<Fact, bool>>>(predicate => predicate.Compile().Invoke(fact1)),
                    default)),
                () => this.mockRepositoryWrapper.Verify(repo => repo.FactRepository.GetAllAsync(
                    It.Is<Expression<Func<Fact, bool>>>(predicate => !predicate.Compile().Invoke(otherFact)),
                    default)));
        }

        [Fact]
        public async Task Handle_Should_ReturnsMappedFacts_WhenRepositoryReturnsData()
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
            var result = await handler.Handle(new GetFactByStreetcodeIdQuery(this.facts[0].StreetcodeId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(this.mappedFacts, result.Value));
        }

        [Fact]
        public async Task Handle_Should_ReturnErrorMessage_WhenRepositoryReturnsNull()
        {
            // Arrange
            Fact fact = this.facts[0];

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
                new GetFactByStreetcodeIdQuery(fact.StreetcodeId),
                CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal($"{ERRORMESSAGE}{fact.StreetcodeId}", result.Errors.FirstOrDefault()?.Message));
        }
    }
}
