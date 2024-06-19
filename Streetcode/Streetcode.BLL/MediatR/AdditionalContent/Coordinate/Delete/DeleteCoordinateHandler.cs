using FluentResults;
using MediatR;
using Streetcode.BLL.Resources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Delete;

public class DeleteCoordinateHandler : IRequestHandler<DeleteCoordinateCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public DeleteCoordinateHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<Unit>> Handle(DeleteCoordinateCommand request, CancellationToken cancellationToken)
    {
        var streetcodeCoordinate = await _repositoryWrapper.StreetcodeCoordinateRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (streetcodeCoordinate is null)
        {
            var errorMsgNull = MessageResourceContext.GetMessage(ErrorMessages.EntityWithIdNotFound, request, request.Id);

            return Result.Fail(new Error(errorMsgNull));
        }

        _repositoryWrapper.StreetcodeCoordinateRepository.Delete(streetcodeCoordinate);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        var errorMsg = MessageResourceContext.GetMessage(ErrorMessages.FailToDeleteA, request);
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error(errorMsg));
    }
}