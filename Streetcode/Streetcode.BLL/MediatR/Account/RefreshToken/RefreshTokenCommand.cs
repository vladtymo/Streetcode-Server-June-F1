using FluentResults;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.DTO.Users;
using Streetcode.DAL.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.MediatR.Account.RefreshTokens
{
    public record RefreshTokensCommand(TokenResponseDTO tokenResponse) : IValidatableRequest<Result<User>>;
}
