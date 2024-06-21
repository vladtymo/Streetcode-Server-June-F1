using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Behavior;
using Streetcode.DAL.Entities.News;

namespace Streetcode.BLL.MediatR.Newss.Update
{
    public record UpdateNewsCommand(NewsDTO news) : IValidatableRequest<Result<NewsDTO>>;
}
