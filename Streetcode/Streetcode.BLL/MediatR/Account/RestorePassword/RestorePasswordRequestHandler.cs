using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.URL;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Entities.AdditionalContent.Email;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.MediatR.Account.RestorePassword
{
    public class RestorePasswordRequestHandler : IRequestHandler<RestorePasswordRequest, Result<string>>
    {        
        private readonly ILoggerService _loggerService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly EmailConfiguration _emailConfiguration;
        private readonly IURLGenerator _urlGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RestorePasswordRequestHandler(
            IMapper mapper,
            ILoggerService logger,
            IEmailService emailService,
            IURLGenerator uRLGenerator,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            EmailConfiguration emailConfiguration)
        {           
            _loggerService = logger;
            _urlGenerator = uRLGenerator;
            _userManager = userManager;
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<string>> Handle(RestorePasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.restPassDto.Email);

            // User not found request for pass change aborted
            if (user == null)
            {
                var error = string.Format(ErrorMessages.UserWithEmailNotFound, request.restPassDto.Email);

                _loggerService.LogError(request, error);

                return Result.Fail(error);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (token == null) 
            {
                var error = ErrorMessages.FailToGenRestorePassToken;

                _loggerService.LogError(request, error);

                return Result.Fail(error);
            }

            var url = _urlGenerator.Url(
                    "RestorePassword",
                    "Account",
                    new 
                    {
                    userId = user.Id, 
                    restToken = token 
                    },
                    _httpContextAccessor.HttpContext!);

            Message msg = new Message(
                new List<string>() { request.restPassDto.Email },
                _emailConfiguration.From,
                "Streetcode Restore Password Request",
                "You have requested the restore password action.\n" +
                "Please use this link for Password Reset: \n" +
                $"{url}\n\t <b>If you don't request Password Restore, than do nothing and check your Accout!</b>");

            bool success = await _emailService.SendEmailAsync(msg);

            if (!success)
            {
                
            }

            return Result.Ok("Email for restoring password was sent successfuly.");
        }
    }
}
