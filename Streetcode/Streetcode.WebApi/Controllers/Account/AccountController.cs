﻿using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.MediatR.Account.Delete;
using Streetcode.BLL.MediatR.Account.Login;
using Streetcode.BLL.MediatR.Account.Logout;
using Streetcode.BLL.MediatR.Account.RefreshToken;
using Streetcode.BLL.MediatR.Account.Register;

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
        public async Task<ActionResult<string>> RefreshTokens()
        {
            return HandleResult(await Mediator.Send(new RefreshTokensCommand()));
        }
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            return HandleResult(await Mediator.Send(new LogoutUserCommand()));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginUser)
        {
            return HandleResult(await Mediator.Send(new LoginUserCommand(loginUser)));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteUserDto deleteUser)
        {
            return HandleResult(await Mediator.Send(new DeleteUserCommand(deleteUser)));
        }
    }
}
