using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Comment;
using Streetcode.BLL.MediatR.Comments.Create;
using Streetcode.BLL.MediatR.Comments.GetAll;
using Streetcode.BLL.MediatR.Comments.GetByUserId;

namespace Streetcode.WebApi.Controllers.Comment
{
    public class CommentController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentCreateDTO comment)
        {
            return HandleResult(await Mediator.Send(new CreateCommentCommand(comment)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllCommentsQuery()));
        }

        [HttpGet]
        public async Task<IActionResult> GetByUserId([FromQuery] Guid userId)
        {
            return HandleResult(await Mediator.Send(new GetCommentsByUserIdQuery(userId)));
        }

        [HttpGet("{streetcodeId:int}")]
        public async Task<IActionResult> GetAllByStreetcodeId([FromRoute] int streetcodeId)
        {
            return HandleResult(await Mediator.Send(new GetAllCommentsByStreetcodeIdQuery(streetcodeId)));
        }
    }
}
