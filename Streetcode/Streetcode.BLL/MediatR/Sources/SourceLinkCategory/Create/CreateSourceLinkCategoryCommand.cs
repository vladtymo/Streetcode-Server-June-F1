using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Sources.Create;
using Streetcode.BLL.Validations;

namespace Streetcode.BLL.MediatR.Sources.SourceLinkCategory.Create;

public record CreateSourceLinkCategoryCommand(CreateSourceCategoryDTO newCategory) : IValidatableRequest<Result<SourceLinkCategoryDTO>>;