﻿using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.MediatR.Account.Register;
using Streetcode.BLL.MediatR.Partners.Create;

namespace Streetcode.WebApi.Controllers.Account
{
    public class AccountController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO newUser)
        {
            return HandleResult(await Mediator.Send(new RegisterUserCommand(newUser)));
        }
    }
}
