using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Email;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.MediatR.Email.ConfirmEmail;
using Streetcode.BLL.MediatR.Email.SendEmail;

namespace Streetcode.WebApi.Controllers.Email
{
    public class EmailController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] EmailDTO email)
        {
            return HandleResult(await Mediator.Send(new SendEmailCommand(email)));
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            return HandleResult(await Mediator.Send(new ConfirmUserEmailCommand(userId, token)));
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromQuery] string email)
        {
            return HandleResult(await Mediator.Send(new SendVerificationEmailCommand(email)));
        }
    }
}
