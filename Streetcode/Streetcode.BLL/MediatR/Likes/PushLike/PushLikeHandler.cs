using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.DTO.Likes;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Entities.Likes;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.Likes.PushLike
{
    public class PushLikeHandler : IRequestHandler<PushLikeCommand, Result<LikeDTO>>
    {
        private IRepositoryWrapper _wrapper;
        private ILoggerService _logger;
        private IMapper _mapper;
        private UserManager<User> _userManager;

        public PushLikeHandler(IRepositoryWrapper wrapper, IMapper mapper, ILoggerService logger, UserManager<User> userManager)
        {
            _wrapper = wrapper;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<LikeDTO>> Handle(PushLikeCommand request, CancellationToken cancellationToken)
        {
            var user = _userManager.FindByIdAsync(request.userId.ToString());

            if(user is null)
            {
                var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.UserNotFound, request);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var like = new Like
            {
                UserId = request.userId,
                streetcodeId = request.streetcodeId,
                CreationTime = DateTime.UtcNow,
            };

            var streetcode = await _wrapper.StreetcodeRepository.GetFirstOrDefaultAsync(u => u.Id == request.streetcodeId);

            if(streetcode is null)
            {
                var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.StreetcodeNotExist, request);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }

            var isLikeExist = await _wrapper.LikeRepository.GetFirstOrDefaultAsync(u => u.UserId == request.userId && u.streetcodeId == request.streetcodeId);
            if (isLikeExist == null)
            {
                _wrapper.LikeRepository.Create(like);
                streetcode.LikesCount++;
                _wrapper.StreetcodeRepository.Update(streetcode);
            } 
            else
            {
                _wrapper.LikeRepository.Delete(isLikeExist);
                streetcode.LikesCount--;
                _wrapper.StreetcodeRepository.Update(streetcode);
            }

            bool isSuccess = await _wrapper.SaveChangesAsync() > 0;
            if(isSuccess)
            {
                return Result.Ok(_mapper.Map<LikeDTO>(like));
            } 
            else
            {
                var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.FailToCreateA, request);
                _logger.LogError(request, errorMsg);
                return Result.Fail(new Error(errorMsg));
            }
        }
    }
}
