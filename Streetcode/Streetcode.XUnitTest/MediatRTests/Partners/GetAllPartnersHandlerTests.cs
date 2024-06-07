using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners
{
    public class GetAllPartnersHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _wrapperMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private List<Partner> partners;
        private List<PartnerDTO> partnersDTO;
        private readonly PartnerDTO _partnerDTO = new PartnerDTO()
        {
            Id = 1,
            LogoId = 12,
            IsKeyPartner = true,
            IsVisibleEverywhere = true,
            Title = "Test",
        };

        private readonly Partner _partner = new Partner()
        {
            Id = 1,
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

        public GetAllPartnersHandlerTests()
        {
            this.partners = new List<Partner>();
            this.partnersDTO = new List<PartnerDTO>();
            this.partners.Add(this._partner);
            this.partnersDTO.Add(this._partnerDTO);
            this._wrapperMock = new Mock<IRepositoryWrapper>();
            this._mapperMock = new Mock<IMapper>();
            this._loggerMock = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task GetAllPartnersHandler_Handle_InsertTrue_ReturnTrue()
        {
            // Arrange
            this._mapperMock.Setup(mapper => mapper.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<object>>())).Returns(this.partnersDTO);
            this._wrapperMock.Setup(repo => repo.PartnersRepository.GetAllAsync(default, default)).ReturnsAsync(this.partners);
            var handlerObj = new GetAllPartnersHandler(this._wrapperMock.Object, this._mapperMock.Object, this._loggerMock.Object);

            // Act
            var result = await handlerObj.Handle(new GetAllPartnersQuery(), default);

            // Assert
            Assert.Equal(this.partners.Count(), result.Value.Count());
        }

        [Fact]
        public async Task GetAllPartnersHandler_Handle_InsertFalse_ReturnFalse()
        {
            // Arrange
            this._mapperMock.Setup(mapper => mapper.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<PartnerDTO>>())).Returns(new List<PartnerDTO>());
            this._wrapperMock.Setup(obj => obj.PartnersRepository.GetAllAsync(default, default)).ReturnsAsync(new List<Partner>());
            var handlerObj = new GetAllPartnersHandler(this._wrapperMock.Object, this._mapperMock.Object, this._loggerMock.Object);

            // Act
            var result = await handlerObj.Handle(new GetAllPartnersQuery(), default);

            // Assert
            Assert.Empty(result.Value);
        }
    }
}