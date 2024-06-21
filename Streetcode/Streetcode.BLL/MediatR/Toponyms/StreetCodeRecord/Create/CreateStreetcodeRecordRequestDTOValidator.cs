using FluentValidation;

namespace Streetcode.BLL.MediatR.Toponyms.StreetCodeRecord.Create;

public class CreateStreetcodeRecordRequestDTOValidator : AbstractValidator<CreateStreetcodeRecordCommand>
{
    public CreateStreetcodeRecordRequestDTOValidator()
    {
        RuleFor(x => x.newRecord).NotNull();
        RuleFor(x => x.newRecord.StreetcodeId).GreaterThan(0).NotNull();
        RuleFor(x => x.newRecord.ToponymId).GreaterThan(0).NotNull();
    }
}