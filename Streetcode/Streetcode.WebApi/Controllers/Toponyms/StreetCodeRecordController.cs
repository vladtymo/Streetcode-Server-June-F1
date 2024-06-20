using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.MediatR.Toponyms.StreetCodeRecord.Create;

namespace Streetcode.WebApi.Controllers.Toponyms
{
    public class StreetCodeRecordController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StreetcodeRecordDTO fact)
        {
            return HandleResult(await Mediator.Send(new CreateStreetcodeRecordCommand(fact)));
        }
    }
}
