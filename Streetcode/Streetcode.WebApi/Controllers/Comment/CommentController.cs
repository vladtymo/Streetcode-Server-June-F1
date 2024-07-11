using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Comment;
using Streetcode.BLL.MediatR.Comments.Create;

namespace Streetcode.WebApi.Controllers.Comment
{
    public class CommentController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentCreateDTO comment)
        {
            return HandleResult(await Mediator.Send(new CreateCommentCommand(comment)));
        }
    }
}
