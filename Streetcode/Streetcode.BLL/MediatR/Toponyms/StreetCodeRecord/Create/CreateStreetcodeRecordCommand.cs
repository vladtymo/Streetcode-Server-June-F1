using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Behavior;


namespace Streetcode.BLL.MediatR.Toponyms.StreetCodeRecord.Create
{
    public record CreateStreetcodeRecordCommand(StreetcodeRecordDTO newRecord) : IValidatableRequest<Result<StreetcodeRecordDTO>>;
}
