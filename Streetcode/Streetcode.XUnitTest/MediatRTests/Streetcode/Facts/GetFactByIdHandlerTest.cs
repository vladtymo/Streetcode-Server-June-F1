// <copyright file="GetFactByIdHandlerTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Streetcode.XUnitTest.MediatRTests.StreetcodeTests.Facts;

using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetById;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

public class GetFactByIdHandlerTest
{
    private const string ERRORMESSAGE = "Cannot find any fact with corresponding id: ";

    private readonly Mock<IRepositoryWrapper> mockRepositoryWrapper;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly List<Fact> facts;
    private readonly List<FactDto> mappedFacts;

    public GetFactByIdHandlerTest()
    {
        this.mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.facts = new List<Fact>
        {
            new Fact { Id = 1, Title = "Test Title", FactContent = "Test Content" },
            new Fact { Id = 2, Title = "Test Title2", FactContent = "Test Content2" },
        };
        this.mappedFacts = new List<FactDto>()
        {
            new FactDto { Id = 1, Title = "Test Title", FactContent = "Test Content" },
            new FactDto { Id = 2 },
        };
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenRepositoryHasCorrectParameters()
    {
        // Arrange
        Fact fact = this.facts[0];
        Fact otherFact = this.facts[1];

        this.mockRepositoryWrapper.
            Setup(repo => repo.FactRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                default))
            .ReturnsAsync(fact);
        var handler = new GetFactByIdHandler(
            this.mockRepositoryWrapper.Object,
            this.mockMapper.Object,
            this.mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetFactByIdQuery(fact.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.True(result.IsSuccess),
            () => this.mockRepositoryWrapper.Verify(repo => repo.FactRepository.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Fact, bool>>>(predicate => predicate.Compile().Invoke(fact)),
                default)),
            () => this.mockRepositoryWrapper.Verify(repo => repo.FactRepository.GetFirstOrDefaultAsync(
                It.Is<Expression<Func<Fact, bool>>>(predicate => !predicate.Compile().Invoke(otherFact)),
                default)));
    }

    [Fact]
    public async Task Handle_Should_ReturnMappedFacts_WhenRepositoryReturnsData()
    {
        // Arrange
        Fact fact = this.facts[0];
        FactDto mappedFact = this.mappedFacts[0];

        this.mockRepositoryWrapper.
             Setup(repo => repo.FactRepository
             .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Fact, bool>>>(), default))
             .ReturnsAsync(fact);
        this.mockMapper.Setup(mapper => mapper.Map<FactDto>(fact))
            .Returns(mappedFact);

        var handler = new GetFactByIdHandler(
            this.mockRepositoryWrapper.Object,
            this.mockMapper.Object,
            this.mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetFactByIdQuery(fact.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
       () => Assert.True(result.IsSuccess),
       () => Assert.Equal(mappedFact.Id, result.Value.Id));
    }

    [Fact]
    public async Task Handle_Should_ReturnErrorMessage_WhenRepositoryReturnsNull()
    {
        // Arrange
        this.mockRepositoryWrapper
            .Setup(repo => repo.FactRepository
            .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Fact, bool>>>(), default))
            .ReturnsAsync((Fact)null!);

        var handler = new GetFactByIdHandler(
            this.mockRepositoryWrapper.Object,
            this.mockMapper.Object,
            this.mockLogger.Object);

        // Act
        var result = await handler.Handle(
            new GetFactByIdQuery(this.facts[0].Id),
            CancellationToken.None);

        // Assert
        Assert.Multiple(
        () => Assert.True(result.IsFailed),
        () => Assert.Equal($"{ERRORMESSAGE}{this.facts[0].Id}", result.Errors.FirstOrDefault()?.Message));
    }
}