using Microsoft.AspNetCore.Mvc;
using RentIt.Application.Exceptions;

namespace RentIt.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate m_Next;
    private readonly ILogger<ExceptionHandlingMiddleware> m_Logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        m_Next = next;
        m_Logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await m_Next(context);
        }
        catch (Exception exception)
        {
            m_Logger.LogError(exception, "Exception occured: {Message}", exception.Message);
            var exceptionDetails = GetExceptionDetails(exception);

            var problemDetails = new ProblemDetails
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail,
            };

            if (exceptionDetails.Errors is not null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }

            context.Response.StatusCode = exceptionDetails.Status;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "ValidationFailure",
                "Validation error",
                "One or more validation errors has occured",
                validationException.Errors),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "ServerError",
                "Server error",
                "An unexpected error has occured",
                null)
        };
    }

    internal record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors);
}
