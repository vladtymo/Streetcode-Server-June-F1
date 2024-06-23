using FluentResults;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Behavior;

namespace Streetcode.BLL.MediatR.Newss.Update
{
    public record UpdateNewsCommand(NewsDTO news) : IValidatableRequest<Result<NewsDTO>>;
}
