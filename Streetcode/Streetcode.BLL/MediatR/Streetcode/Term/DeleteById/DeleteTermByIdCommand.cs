using FluentResults;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Term.DeleteById
{
    public record DeleteTermByIdCommand(int Id) : IValidatableRequest<Result<TermDTO>>;
}
