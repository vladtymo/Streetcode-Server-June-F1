using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Account.RefreshToken;
using Streetcode.DAL.Entities.Users;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Account.RefreshToken
{
    public class RefreshTokenTests
    {
        private readonly Mock<ILoggerService> _logger;
        private readonly Mock<HttpRequest> _request;
        private readonly Mock<UserManager<User>> _userManager;

        public RefreshTokenTests()
        {
            _logger = new Mock<ILoggerService>();
            _request = new Mock<HttpRequest>();
            _userManager = new Mock<UserManager<User>>();
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccessResult_WhenReturnToken()
        {
            // Arrange
            _request.Setup(obj => obj.Cookies["refreshToken"]).Returns("token");
            var comm = new RefreshTokenCommand(new User() { RefreshToken = "token", Expires = DateTime.Now.AddDays(7) });
            var handler = new RefreshTokenHandler(_request.Object, _logger.Object, _userManager.Object);

            // Act
            var result = await handler.Handle(comm, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
