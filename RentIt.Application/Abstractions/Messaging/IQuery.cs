using MediatR;
using RentIt.Domain.Abstractions;

namespace RentIt.Application.Abstractions.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
