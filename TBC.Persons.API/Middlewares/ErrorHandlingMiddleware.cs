using System.Net;
using System.Text.Json;
using FluentValidation;
using TBC.Persons.Domain.Messages;

namespace TBC.Persons.API.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        switch (ex)
        {
            case ValidationException validationException:
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                var res = new ValidationFailedApiServiceResponse(validationException.Errors.FirstOrDefault().ErrorCode);

                foreach (var item in validationException.Errors)
                {
                    res.AddError($"{item.ErrorCode} : {item.ErrorMessage[0]}");
                }

                return context.Response.WriteAsync(JsonSerializer.Serialize(res));
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(
                    JsonSerializer.Serialize(new InternalServiceFailedApiServiceResponse(ex)));
        }
    }
}