using FluentResults;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Account.Register
{
    public record RegisterUserCommand(UserRegisterDTO newUser) : IValidatableRequest<Result<UserDTO>>;
}
