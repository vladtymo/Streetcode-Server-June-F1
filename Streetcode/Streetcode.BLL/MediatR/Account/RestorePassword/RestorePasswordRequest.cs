using FluentResults;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Account.RestorePassword
{
    public record class RestorePasswordRequest(RequestRestorePasswordDto restPassDto) : IValidatableRequest<Result<string>>;  
}
