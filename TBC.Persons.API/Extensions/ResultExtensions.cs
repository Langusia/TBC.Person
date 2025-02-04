using Microsoft.AspNetCore.Mvc;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Messages;

namespace TBC.Persons.API.Extensions;

public static class ResultExtensions
{
    public static async Task<ActionResult<ApiServiceResponse>> Match(
        this Task<Result> resultTask,
        Func<ActionResult<ApiServiceResponse>> onSuccess,
        Func<Result, ActionResult<ApiServiceResponse>> onFailure
    )
    {
        var result = await resultTask;

        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static async Task<ActionResult<ApiServiceResponse<TIn>>> Match<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, ActionResult<ApiServiceResponse<TIn>>> onSuccess,
        Func<Result, ActionResult<ApiServiceResponse<TIn>>> onFailure
    )
    {
        var result = await resultTask;

        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}