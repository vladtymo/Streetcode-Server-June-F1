using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Comment;
using Streetcode.BLL.MediatR.Replies;

namespace Streetcode.WebApi.Controllers.Reply;

public class ReplyController : BaseApiController
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReplyCreateDTO reply)
    {
        return HandleResult(await Mediator.Send(new CreateReplyCommand(reply)));
    }
}


