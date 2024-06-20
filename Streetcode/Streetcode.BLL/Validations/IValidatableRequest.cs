using MediatR;

namespace Streetcode.BLL.Validations;

public interface IValidatableRequest<out TResponse> : IRequest<TResponse>, IValidatableRequest
{
}

public interface IValidatableRequest
{
}