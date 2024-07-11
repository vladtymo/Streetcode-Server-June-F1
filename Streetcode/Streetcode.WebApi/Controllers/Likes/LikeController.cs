using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Instagram.GetAll;
using Streetcode.BLL.MediatR.Likes.PushLike;

namespace Streetcode.WebApi.Controllers.Likes
{
    public class LikeController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> PushLike([FromBody] PushLikeCommand command)
        {
            return HandleResult(await Mediator.Send(new PushLikeCommand(command.userId, command.streetcodeId)));
        }
    }
}
