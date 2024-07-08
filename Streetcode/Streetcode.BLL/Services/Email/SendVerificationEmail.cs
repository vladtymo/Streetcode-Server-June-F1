using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.URL;
using Streetcode.DAL.Entities.AdditionalContent.Email;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Services.Email
{
    public class SendVerificationEmail : ISendVerificationEmail
    {
        private const string ACTION = "ConfirmEmail";
        private const string CONTROLLER = "Account";
        private const string SUBJECT = "Confirm your email";
        private const string FROM = "Streetcode";

        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailSender;
        private readonly IURLGenerator _urlGenerator;
        private readonly IHttpContextAccessor _contextAccessor;

        public SendVerificationEmail(
            UserManager<User> userManager, 
            IEmailService emailSender, 
            IURLGenerator urlGenerator, 
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _urlGenerator = urlGenerator;
            _contextAccessor = contextAccessor;
        }

        public async Task SendVerification(string email)
        {
            string url = await CreateUrl(email);
            await SendEmail(email, url);
        }

        private async Task<string> CreateUrl(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return _urlGenerator.Url(ACTION, CONTROLLER, new { userId = user.Id, token }, _contextAccessor.HttpContext!);
            }
            else
            {
                throw new Exception("User not found");
            }
        }

        private async Task SendEmail(string email, string url)
        {
            await _emailSender
                    .SendEmailAsync(
                    new Message(new List<string> { email }, FROM, SUBJECT, url!));
        }
    }
}
