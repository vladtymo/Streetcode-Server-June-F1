using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.Services.Cache;
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
        private readonly Mock<ICacheService> _cacheServiceMock;
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
            partners = new List<Partner>();
            partnersDTO = new List<PartnerDTO>();
            partners.Add(_partner);
            partnersDTO.Add(_partnerDTO);
            _wrapperMock = new Mock<IRepositoryWrapper>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILoggerService>();
            _cacheServiceMock = new Mock<ICacheService>();
        }

        [Fact]
        public async Task Handle_Should_ReturnEqualTrue_WhenGetTruePartner()
        {
            // Arrange
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<object>>())).Returns(partnersDTO);
            _wrapperMock.Setup(repo => repo.PartnersRepository.GetAllAsync(default, default)).ReturnsAsync(partners);
            var handlerObj = new GetAllPartnersHandler(_wrapperMock.Object, _mapperMock.Object, _loggerMock.Object, _cacheServiceMock.Object);

            // Act
            var result = await handlerObj.Handle(new GetAllPartnersQuery(), default);

            // Assert
            Assert.Equal(partners.Count(), result.Value.Count());
        }

        [Fact]
        public async Task Handle_Should_ReturnEmpty_WhenGetEmptyPartner()
        {
            // Arrange
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<PartnerDTO>>())).Returns(new List<PartnerDTO>());
            _wrapperMock.Setup(obj => obj.PartnersRepository.GetAllAsync(default, default)).ReturnsAsync(new List<Partner>());
            var handlerObj = new GetAllPartnersHandler(_wrapperMock.Object, _mapperMock.Object, _loggerMock.Object, _cacheServiceMock.Object);

            // Act
            var result = await handlerObj.Handle(new GetAllPartnersQuery(), default);

            // Assert
            Assert.Empty(result.Value);
        }
    }
}