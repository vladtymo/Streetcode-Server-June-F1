using FluentValidation;

namespace Streetcode.BLL.MediatR.Toponyms.StreetCodeRecord.Delete;

public class DeleteStreetcodeRecordRequestDTOValidator : AbstractValidator<DeleteStreetcodeRecordCommand>
{
    public DeleteStreetcodeRecordRequestDTOValidator()
    {
        RuleFor(x => x.StreetcodeId).GreaterThan(0).NotNull();
        RuleFor(x => x.ToponymId).GreaterThan(0).NotNull();
    }
}