using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.MediatR.Toponyms.StreetCodeRecord.Delete
{
    public record DeleteStreetcodeRecordQuery(int StreetcodeId, int ToponymId) : IRequest<Result<StreetcodeRecordDTO>>;
}
