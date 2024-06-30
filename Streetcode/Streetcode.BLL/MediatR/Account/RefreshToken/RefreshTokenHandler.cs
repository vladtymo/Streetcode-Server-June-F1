using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SoftServerCinema.Security.Interfaces;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.BLL.Services.Tokens;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.MediatR.Account.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Result<string>>
    {
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;

        public RefreshTokenHandler(ILoggerService logger, TokenService tokenService, IMapper mapper)
        {
            _logger = logger;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.user);
            if(user == null)
            {
                var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.UserNotFound);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            _tokenService.GenerateTokens(user);

            return Result.Ok();
        }
    }
}
