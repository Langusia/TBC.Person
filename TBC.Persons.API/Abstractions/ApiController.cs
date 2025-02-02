using MediatR;
using Microsoft.AspNetCore.Mvc;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Enums;
using TBC.Persons.Domain.Messages;

namespace TBC.Persons.API.Abstractions;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender) => Sender = sender;

    // Non-Generic HandleFailure
    protected ActionResult<ApiServiceResponse> HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            { IsSuccess: false, Errors: var errors } =>
                errors[0].ErrorType switch
                {
                    ErrorTypeEnum.UnprocessableEntity =>
                        new BadRequestObjectResult(new BadRequestApiServiceResponse
                        {
                            ValidationErrors = errors.Select(error => $"{error.Code} {error.Message}").ToList()
                        }),
                    ErrorTypeEnum.NoContent => new NoContentResult(),
                    ErrorTypeEnum.NotFound =>
                        new NotFoundObjectResult(new NotFoundApiServiceResponse
                        {
                            Message = string.Join(',', errors.Select(error => $"{error.Code} - {error.Message}"))
                        }),
                    ErrorTypeEnum.BadRequest =>
                        new BadRequestObjectResult(new BadRequestApiServiceResponse
                        {
                            Message = string.Join(',', errors.Select(error => $"{error.Code} - {error.Message}"))
                        }),
                    _ =>
                        new BadRequestObjectResult(new BadRequestApiServiceResponse
                        {
                            Message = string.Join(',', errors.Select(error => $"{error.Code} - {error.Message}"))
                        }),
                },
            _ => new BadRequestObjectResult(new BadRequestApiServiceResponse())
        };

    protected ActionResult<ApiServiceResponse<T>> HandleFailure<T>(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            { IsSuccess: false, Errors: var errors } =>
                errors[0].ErrorType switch
                {
                    ErrorTypeEnum.UnprocessableEntity =>
                        new BadRequestObjectResult(
                            new BadRequestApiServiceResponse<T>()
                            {
                                ValidationErrors = errors.Select(error => $"{error.Code} {error.Message}").ToList()
                            }
                        ),
                    ErrorTypeEnum.NoContent => new NoContentResult(),
                    ErrorTypeEnum.NotFound =>
                        new NotFoundObjectResult(
                            new NotFoundApiServiceResponse<T>()
                            {
                                Message = string.Join(',', errors.Select(error => $"{error.Code} - {error.Message}"))
                            }
                        ),
                    ErrorTypeEnum.BadRequest =>
                        new BadRequestObjectResult(
                            new BadRequestApiServiceResponse<T>()
                            {
                                Message = string.Join(',', errors.Select(error => $"{error.Code} - {error.Message}"))
                            }
                        ),
                    _ =>
                        new BadRequestObjectResult(
                            new BadRequestApiServiceResponse<T>()
                            {
                                Message = string.Join(',', errors.Select(error => $"{error.Code} - {error.Message}"))
                            }
                        ),
                },
            _ =>
                new BadRequestObjectResult(
                    new BadRequestApiServiceResponse<T>()
                )
        };
}