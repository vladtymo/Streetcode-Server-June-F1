using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.BLL.MediatR.Account.Logout;
using Streetcode.BLL.Services.CacheService;
using Streetcode.DAL.Entities.Users;
using Xunit;
namespace Streetcode.XUnitTest.MediatRTests.Account.Logout;

 public class LogoutUserHandlerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly LogoutUserHandler _handler;
        
        public LogoutUserHandlerTests()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILoggerService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _cacheServiceMock = new Mock<ICacheService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _handler = new LogoutUserHandler(
                _userManagerMock.Object,
                _cacheServiceMock.Object,
                _loggerMock.Object,
                _httpContextAccessorMock.Object,
                _mapperMock.Object);
        }

    [Fact]
    public async Task Handle_ShouldLogoutUserSuccessfully()
    {
        // Arrange
        var userId = "d0d3c3b1-4b3e-4b3e-8b3e-4b3e8b3e4b3e";
        var userEmail = "test@example.com";
        var accessToken = "valid-access-token";

        var user = new User { Id = Guid.Parse(userId), Email = userEmail, RefreshToken = "refresh-token" };

        var claims = new List<Claim> 
        { 
            new Claim(ClaimTypes.Email, userEmail) 
        };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = $"Bearer {accessToken}";
        httpContext.User = principal;

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
        _userManagerMock.Setup(x => x.FindByEmailAsync(userEmail)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _cacheServiceMock.Setup(x => x.SetBlacklistedTokenAsync(accessToken, userId)).ReturnsAsync(true);
        _mapperMock.Setup(x => x.Map<UserDTO>(It.IsAny<User>())).Returns(new UserDTO { Email = userEmail });

        var request = new LogoutUserCommand();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Email.Should().Be(userEmail);

        _httpContextAccessorMock.Verify(x => x.HttpContext, Times.Once);
        _userManagerMock.Verify(x => x.FindByEmailAsync(userEmail), Times.Once);
        _userManagerMock.Verify(x => x.UpdateAsync(It.Is<User>(u => u.Email == userEmail && u.RefreshToken == null!)), Times.Once);
        _cacheServiceMock.Verify(x => x.SetBlacklistedTokenAsync(accessToken, userId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenAccessTokenIsNotProvided()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var request = new LogoutUserCommand();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        _loggerMock.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
