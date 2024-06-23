using FluentResults;
using MediatR;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Newss.Delete
{
    public record DeleteNewsCommand(int id) : IValidatableRequest<Result<Unit>>;
}
