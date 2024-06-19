using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;


namespace Streetcode.BLL.MediatR.Toponyms.StreetCodeRecord.Create
{
    public record CreateStreetcodeRecordCommand(StreetcodeRecordDTO newRecord) : IRequest<Result<StreetcodeRecordDTO>>;
}
