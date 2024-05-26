using MediatR;
using RentIt.Domain.Abstractions;

namespace RentIt.Application.Abstractions.Messaging;
public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}