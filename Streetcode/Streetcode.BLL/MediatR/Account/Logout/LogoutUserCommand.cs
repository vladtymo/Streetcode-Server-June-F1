using FluentResults;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Account.Logout
{
    public record LogoutUserCommand() : IValidatableRequest<Result>;
}
