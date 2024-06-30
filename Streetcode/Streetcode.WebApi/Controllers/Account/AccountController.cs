using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.MediatR.Account.RefreshToken;
using Streetcode.BLL.MediatR.Account.Register;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.Services.Tokens;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.WebApi.Controllers.Account
{
    public class AccountController : BaseApiController
    {

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO newUser)
        {
            return HandleResult(await Mediator.Send(new RegisterUserCommand(newUser)));
        }

        [HttpPost]
        public async Task<ActionResult<string>> RefreshToken(User user)
        {
            return HandleResult(await Mediator.Send(new RefreshTokenCommand(user)));
        }
    }
}
