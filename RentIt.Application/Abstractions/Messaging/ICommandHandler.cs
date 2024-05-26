using MediatR;
using RentIt.Application.Abstractions.Messaging;
using RentIt.Domain.Abstractions;

namespace RentIt.Application.Abstractions.Messaging;
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>, IBaseCommand
    where TCommand : ICommand<TResponse>
{
}
