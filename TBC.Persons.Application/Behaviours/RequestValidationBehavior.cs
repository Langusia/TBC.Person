﻿using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using MediatR;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Application.Behaviours;

[ExcludeFromCodeCoverage]
public class RequestValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result 
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        Error[] errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error(
                    failure.PropertyName,
                    failure.ErrorMessage,
                    ErrorTypeEnum.UnprocessableEntity
                )
            )
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }

    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (Result.Failure(errors) as TResult)!;
        }

        var result = typeof(Result)
            .GetMethods()
            .First(m =>
                m is { IsGenericMethod: true, Name: nameof(Result.Failure) } &&
                m.GetParameters().First().ParameterType == typeof(Error[]))!
            .MakeGenericMethod(typeof(TResult).GenericTypeArguments[0])
            .Invoke(null, new object?[] { errors })!;

        return (TResult)result;
    }
}
