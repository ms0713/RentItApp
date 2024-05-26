using MediatR;
using Microsoft.Extensions.Logging;
using RentIt.Application.Abstractions.Messaging;

namespace RentIt.Application.Abstractions.Behavior;
public class LoggingBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> m_Logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        m_Logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    { 
        var name = request.GetType().Name;

        try
        {
            m_Logger.LogInformation("Executing command {Command}", name);

            var result = await next();

            m_Logger.LogInformation("Command {Command} processed successfully", name);

            return result;

        }
        catch (Exception ex)
        {
            m_Logger.LogError(ex, "Command {Command} processing failed", name);
            throw;
        }
    }
}
