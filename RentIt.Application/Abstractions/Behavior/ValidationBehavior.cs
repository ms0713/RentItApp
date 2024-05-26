using FluentValidation;
using MediatR;
using RentIt.Application.Abstractions.Messaging;
using RentIt.Application.Exceptions;

namespace RentIt.Application.Abstractions.Behavior;
public class ValidationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> m_Validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        m_Validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (m_Validators.Any() == false)
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = m_Validators
            .Select(validator => validator.Validate(context))
            .Where(validatioResult => validatioResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage))
            .ToList();

        if (validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        }

        return await next();
    }
}
