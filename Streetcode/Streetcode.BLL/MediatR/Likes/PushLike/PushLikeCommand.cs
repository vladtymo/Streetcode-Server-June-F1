using FluentResults;
using Streetcode.BLL.Behavior;
using Streetcode.BLL.DTO.Likes;

namespace Streetcode.BLL.MediatR.Likes.PushLike
{
    public record PushLikeCommand(Guid userId, int streetcodeId) : IValidatableRequest<Result<LikeDTO>>;
}
