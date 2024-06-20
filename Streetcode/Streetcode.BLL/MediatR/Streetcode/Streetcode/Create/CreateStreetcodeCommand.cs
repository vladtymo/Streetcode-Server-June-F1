using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
public record CreateStreetcodeCommand(CreateStreetcodeDTO newStreetcode)
    : IRequest<Result<CreateStreetcodeDTO>>;
