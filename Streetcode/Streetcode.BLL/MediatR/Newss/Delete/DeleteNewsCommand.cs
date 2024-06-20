using FluentResults;
using MediatR;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Newss.Delete
{
    public record DeleteNewsCommand(int id) : IValidatableRequest<Result<Unit>>;
}
