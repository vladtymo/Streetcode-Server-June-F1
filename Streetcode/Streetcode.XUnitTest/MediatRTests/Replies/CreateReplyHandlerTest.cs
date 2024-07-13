using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using Streetcode.BLL.DTO.Comment;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.BLL.MediatR.Comments.Create;
using Streetcode.BLL.MediatR.Replies;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Entities.Comments;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Replies;

public class CreateReplyHandlerTest
{
            private readonly Mock<IMapper> _mockMapper;
            private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
            private readonly Mock<ILoggerService> _mockLogger;
            private readonly Mock<ITokenService> _mockTokenService;
            private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
            private readonly CreateReplyHandler _handler;
            private readonly CreateReplyCommand _command;
            public CreateReplyHandlerTest()
            {
                _mockMapper = new Mock<IMapper>();
                _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
                _mockLogger = new Mock<ILoggerService>();
                _mockTokenService = new Mock<ITokenService>();
                _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
                _handler = new CreateReplyHandler( _mockRepositoryWrapper.Object, _mockMapper.Object,_mockLogger.Object, _mockHttpContextAccessor.Object,_mockTokenService.Object );
                _command = new CreateReplyCommand(new ReplyCreateDTO());
            }

            public async Task Handle_Should_ReturnFailResult_WhenReplyIsNull()
            {
                // Arrange
                _mockMapper.Setup(m => m.Map<Reply>(It.IsAny<ReplyCreateDTO>())).Returns((Reply)null!);

                // Act
                var result = await _handler.Handle(_command, CancellationToken.None);

                // Assert
                Assert.True(result.IsFailed);
                Assert.Equal(MessageResourceContext.GetMessage(ErrorMessages.CanNotMap, _command), result.Errors[0].Message);
            }
            
            [Fact]
            public async Task Handle_Should_ReturnFailResult_WhenAccessTokenIsMissing()
            {
                // Arrange
                _mockMapper.Setup(m => m.Map<Reply>(It.IsAny<ReplyCreateDTO>())).Returns(new Reply());
                var httpContext = new DefaultHttpContext();
                _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

                // Act
                var result = await _handler.Handle(_command, CancellationToken.None);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.True(result.IsFailed);
                    Assert.Equal(
                        MessageResourceContext.GetMessage(ErrorMessages.AccessTokenNotFound, _command),
                        result.Errors[0].Message);
                });
            }
            
            [Fact]
            public async Task Handle_Should_ReturnFailResult_WhenSaveChangesFails()
            {
                // Arrange
                var reply = new Reply();
                var accessToken = "validToken";
                var requestCookies = new Mock<IRequestCookieCollection>();
                requestCookies.Setup(c => c.TryGetValue("accessToken", out accessToken)).Returns(true);

                _mockMapper.Setup(m => m.Map<Reply>(It.IsAny<ReplyCreateDTO>())).Returns(reply);

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Cookies = requestCookies.Object;

                _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);
                _mockTokenService.Setup(t => t.GetUserIdFromAccessToken(It.IsAny<string>())).Returns(Guid.NewGuid().ToString());

                _mockRepositoryWrapper.Setup(r => r.CommentRepository.CreateAsync(It.IsAny<Reply>())).ReturnsAsync(reply);
                _mockRepositoryWrapper.Setup(r => r.SaveChangesAsync()).ReturnsAsync(0);

                // Act
                var result = await _handler.Handle(_command, CancellationToken.None);

                Assert.Multiple(() =>
                {
                    Assert.True(result.IsFailed);
                    Assert.Equal(MessageResourceContext.GetMessage(ErrorMessages.FailToCreateA, _command), result.Errors[0].Message);
                });
            }
            
            [Fact]
            public async Task Handle_Should_ReturnSuccessResult_WhenValidRequest()
            {
                // Arrange
                var reply = new Reply();
                var replyDto = new CommentDTO();
                var accessToken = "validToken";
                var userId = Guid.NewGuid().ToString();

                var requestCookies = new Mock<IRequestCookieCollection>();
                requestCookies.Setup(c => c.TryGetValue("accessToken", out accessToken)).Returns(true);

                _mockMapper.Setup(m => m.Map<Reply>(It.IsAny<ReplyCreateDTO>())).Returns(reply);
                _mockMapper.Setup(m => m.Map<CommentDTO>(It.IsAny<Reply>())).Returns(replyDto);

                var httpContext = new DefaultHttpContext();
                httpContext.Request.Cookies = requestCookies.Object;

                _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);
                _mockTokenService.Setup(t => t.GetUserIdFromAccessToken(accessToken)).Returns(userId);

                _mockRepositoryWrapper.Setup(r => r.CommentRepository.CreateAsync(It.IsAny<Reply>())).ReturnsAsync(reply);
                _mockRepositoryWrapper.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

                // Act
                var result = await _handler.Handle(_command, CancellationToken.None);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.True(result.IsSuccess);
                    Assert.Equal(replyDto, result.Value);
                });
            }
}

