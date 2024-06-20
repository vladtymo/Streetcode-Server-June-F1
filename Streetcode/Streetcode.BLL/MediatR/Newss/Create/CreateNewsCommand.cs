using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Newss.Create
{
    public record CreateNewsCommand(NewsDTO newNews) : IValidatableRequest<Result<NewsDTO>>;
}
