using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Org.BouncyCastle.Asn1.Ocsp;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetById;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners
{
    public class GetPartnerByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _wrapperMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;

        private PartnerDTO _partnerDTO = new PartnerDTO()
        {
            Id = 2,
            LogoId = 12,
            IsKeyPartner = true,
            IsVisibleEverywhere = true,
            Title = "Test",
        };

        private Partner _partner = new Partner()
        {
            Id = 2,
            LogoId = 12,
            IsKeyPartner = true,
            IsVisibleEverywhere = true,
            Title = "Test",
            PartnerSourceLinks = new List<PartnerSourceLink>
            {
                new PartnerSourceLink()
                {
                    LogoType = DAL.Enums.LogoType.Twitter,
                    TargetUrl = "string"
                },
            },
        };
        private Partner _expectedPartner = new Partner()
        {
            Id = 2,
            LogoId = 12,
            IsKeyPartner = true,
            IsVisibleEverywhere = true,
            Title = "Test",
            PartnerSourceLinks = new List<PartnerSourceLink>
            {
                new PartnerSourceLink()
                {
                    LogoType = DAL.Enums.LogoType.Twitter,
                    TargetUrl = "string"
                },
            },
        };

        public GetPartnerByIdHandlerTests()
        {
            this._wrapperMock = new Mock<IRepositoryWrapper>();
            this._mapperMock = new Mock<IMapper>();
            this._loggerMock = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task GetPartnerByIdHandler_Handler_ReturnSuccess()
        {
            // Arrange
            this._mapperMock.Setup(obj => obj.Map<PartnerDTO>(It.IsAny<object>())).Returns(this._partnerDTO);
            this._wrapperMock.Setup(obj => obj.PartnersRepository.GetSingleOrDefaultAsync(default, default)).ReturnsAsync(this._partner);

            var handler = new GetPartnerByIdHandler(this._wrapperMock.Object, this._mapperMock.Object, this._loggerMock.Object);
            var partnerquery = new GetPartnerByIdQuery(this._expectedPartner.Id);

            // Act
            var result = await handler.Handle(partnerquery, default);

            // Assert
            Assert.Equal(this._expectedPartner.Id, result.Value.Id);
        }
    }
}