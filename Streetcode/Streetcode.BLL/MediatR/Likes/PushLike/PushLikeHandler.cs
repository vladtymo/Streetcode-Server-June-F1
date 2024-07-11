using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Likes;
using Streetcode.DAL.Entities.Likes;

namespace Streetcode.BLL.MediatR.Likes.PushLike
{
    public class PushLikeHandler : IRequestHandler<PushLikeCommand, Result<LikeDTO>>
    {
        public Task<Result<LikeDTO>> Handle(PushLikeCommand request, CancellationToken cancellationToken)
        {
            var like = new LikeDTO
            {
                UserId = request.userId,
                streetcodeId = request.streetcodeId,
                CreationTime = DateTime.UtcNow,
            };

            throw new NotImplementedException();
        }
    }
}
