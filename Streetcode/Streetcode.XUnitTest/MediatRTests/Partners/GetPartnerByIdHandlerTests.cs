using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetById;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners
{
    public class GetPartnerByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _wrapperMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private PartnerDTO _partnerDTO = new PartnerDTO() { Id = 2 };
        private Partner _partner = new Partner() { Id = 2 };

        public GetPartnerByIdHandlerTests()
        {
            this._wrapperMock = new Mock<IRepositoryWrapper>();
            this._mapperMock = new Mock<IMapper>();
            this._loggerMock = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handle_Should_ReturnEqualTrue_WhenPartnerEqualToExpected()
        {
            // Arrange
            var expectedId = 2;
            this._mapperMock.Setup(obj => obj.Map<PartnerDTO>(It.IsAny<object>())).Returns(this._partnerDTO);
            this._wrapperMock.Setup(obj => obj.PartnersRepository
                                              .GetSingleOrDefaultAsync(
                                              It.IsAny<Expression<Func<Partner, bool>>>(),
                                              It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
                                              .ReturnsAsync(this._partner);
            var handler = new GetPartnerByIdHandler(this._wrapperMock.Object, this._mapperMock.Object, this._loggerMock.Object);

            // Act
            Result<PartnerDTO> result = await handler.Handle(new GetPartnerByIdQuery(expectedId), CancellationToken.None);

            // Assert
            Assert.Equal(expectedId, result.Value.Id);
        }

        [Fact]
        public async Task Handle_Should_ReturnEqualFalse_WhenPartnerNotEqualToExpected()
        {
            // Arrange
            var expectedId = 3;
            this._mapperMock.Setup(obj => obj.Map<PartnerDTO>(It.IsAny<object>())).Returns(new PartnerDTO());
            this._wrapperMock.Setup(obj => obj.PartnersRepository
                                              .GetSingleOrDefaultAsync(
                                              It.IsAny<Expression<Func<Partner, bool>>>(),
                                              It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
                                              .ReturnsAsync(new Partner());
            var handler = new GetPartnerByIdHandler(this._wrapperMock.Object, this._mapperMock.Object, this._loggerMock.Object);

            // Act
            Result<PartnerDTO> result = await handler.Handle(new GetPartnerByIdQuery(expectedId), CancellationToken.None);

            // Assert
            Assert.NotEqual(expectedId, result.Value.Id);
        }
    }
}