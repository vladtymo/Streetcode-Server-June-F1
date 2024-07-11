using AutoMapper;
using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Likes;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Likes.PushLike
{
    public class PushLikeHandler : IRequestHandler<PushLikeCommand, Result<LikeDTO>>
    {
        private IRepositoryWrapper _wrapper;
        private IMapper _mapper;

        public PushLikeHandler(IRepositoryWrapper wrapper, IMapper mapper)
        {
            _wrapper = wrapper;
            _mapper = mapper;
        }

        public Task<Result<LikeDTO>> Handle(PushLikeCommand request, CancellationToken cancellationToken)
        {
            var like = new LikeDTO
            {
                UserId = request.userId,
                streetcodeId = request.streetcodeId,
                CreationTime = DateTime.UtcNow,
            };

            var isLikeExist = _wrapper.LikeRepository.GetFirstOrDefaultAsync(u => u.UserId == request.userId && u.streetcodeId == request.streetcodeId);
            if (isLikeExist == null)
            {

                // like

            } 
            else
            {

                // unlike

            }

            throw new NotImplementedException();
        }
    }
}
