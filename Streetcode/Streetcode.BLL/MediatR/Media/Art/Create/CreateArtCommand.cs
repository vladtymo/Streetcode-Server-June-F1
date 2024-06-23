using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;

namespace Streetcode.BLL.MediatR.Media.Art.Create
{
    public record CreateArtCommand(ArtCreateUpdateDTO Art) : IRequest<Result<ArtCreateUpdateDTO>>;
}
