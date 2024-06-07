using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.MediatR.Partners.GetById;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners
{
    public class CreatePartnerHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _wrapperMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;

        private CreatePartnerDTO _createPartnerDTO = new CreatePartnerDTO()
        {
            Id = 2,
            LogoId = 12,
            IsKeyPartner = true,
            IsVisibleEverywhere = true,
            Title = "Test",
        };
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

        public CreatePartnerHandlerTests()
        {
            this._wrapperMock = new Mock<IRepositoryWrapper>();
            this._mapperMock = new Mock<IMapper>();
            this._loggerMock = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task CreateHandler_Handler_ReturnSuccess()
        {
            // Arrange
            this._mapperMock.Setup(obj => obj.Map<PartnerDTO>(It.IsAny<object>())).Returns(this._partnerDTO);
            this._wrapperMock.Setup(obj => obj.PartnersRepository.CreateAsync(new Partner())).ReturnsAsync(this._partner);

            var handler = new CreatePartnerHandler(this._wrapperMock.Object, this._mapperMock.Object, this._loggerMock.Object);
            var partnerquery = new CreatePartnerQuery(_createPartnerDTO);

            // Act
            var result = await handler.Handle(partnerquery, default);

            // Assert
            Assert.NotNull(result.Value);
        }
    }
}
