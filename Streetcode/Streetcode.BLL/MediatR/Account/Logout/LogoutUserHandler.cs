using System.Security.Claims;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Services.CacheService;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.MediatR.Account.Logout
{
    public class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICacheService _cacheService;
        private readonly ILoggerService _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutUserHandler(UserManager<User> userManager, SignInManager<User> signInManager,
            ICacheService cacheService, ILoggerService logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cacheService = cacheService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            
            if (string.IsNullOrEmpty(accessToken))
            {
                const string errorMsg = "Cannot find access token";
                _logger.LogError(accessToken!, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
            
            var claims = _httpContextAccessor.HttpContext?.User.Claims.Select(c => c.Value).ToList();
            if(!claims!.Any())
            {
                const string errorMsg = "Cannot find claims";
                _logger.LogError(claims!, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
            
            var user = await _userManager.FindByEmailAsync(claims!.FirstOrDefault(c => c.Contains(ClaimTypes.Email)));
            if (user == null)
            {
                const string errorMsg = "Cannot find a user with this id";
                _logger.LogError(user!, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            user.RefreshToken = null;

            await _signInManager.SignOutAsync();
            await _httpContextAccessor.HttpContext!.SignOutAsync();
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                const string errorMsg = "Failed to update user information";
                _logger.LogError(result!, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var cacheResult = await _cacheService.SetBlacklistedTokenAsync(accessToken, user.Id.ToString());
            if (!cacheResult)
            {
                const string errorMsg = "Failed to blacklist access token";
                _logger.LogError(cacheResult, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            return Result.Ok("User logged out successfully");
        }
    }
}
