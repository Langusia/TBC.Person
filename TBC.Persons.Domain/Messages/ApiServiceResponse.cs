﻿namespace TBC.Persons.Domain.Messages;

public abstract class ApiServiceResponse
{
    public string? Message { get; set; }
    public string? DetailsMessage { get; set; }
    public ExternalApiStatus ExternalState { get; set; }
    public ApiStatus State { get; set; }
    public string? ErrorCode { get; set; }
    public List<string> ValidationErrors { get; set; }

    public bool IsOk() => State == ApiStatus.Ok;
}

public class SuccessApiServiceResponse : ApiServiceResponse
{
    public SuccessApiServiceResponse(string message = null)
    {
        State = ApiStatus.Ok;
        Message = message;
    }
}

public class ValidationFailedApiServiceResponse : ApiServiceResponse
{
    public ValidationFailedApiServiceResponse(string param, string errorCode = ResponseErrorCode.BadRequest)
    {
        ErrorCode = errorCode;
        Message = $"Invalid parameter '{param}'";
        State = ApiStatus.BadRequest;
    }

    public void AddError(string error)
    {
        if (this.ValidationErrors == null)
            this.ValidationErrors = new List<string>();

        this.ValidationErrors.Add(error);
    }
}

public class ExternalServiceFailedApiServiceResponse : ApiServiceResponse
{
    public ExternalServiceFailedApiServiceResponse(string serviceName, string message, string status,
        ExternalApiStatus errorStatus, string errorCode = ResponseErrorCode.ServiceCallError)
    {
        Message = message;
        DetailsMessage = $"External service '{serviceName}' failed: {status} message: {message}";
        State = ApiStatus.Failed;
        ExternalState = errorStatus;
        ErrorCode = errorCode;
    }
}

public class InternalServiceFailedApiServiceResponse : ApiServiceResponse
{
    public InternalServiceFailedApiServiceResponse(string message, string errorCode = ResponseErrorCode.GeneralError)
    {
        Message = $"Internal service error";
        DetailsMessage = $"Internal service error '{message}'";
        State = ApiStatus.Failed;
        ErrorCode = errorCode;
    }

    public InternalServiceFailedApiServiceResponse(Exception ex, string errorCode = ResponseErrorCode.GeneralError)
    {
        //TODO: analyze header trace value

        Message = $"Internal service error";
        DetailsMessage = $"Internal service error '{ex}'";
        State = ApiStatus.Failed;
        ErrorCode = errorCode;
    }
}

public class BadRequestApiServiceResponse : ApiServiceResponse
{
    public BadRequestApiServiceResponse(string message = null, string errorCode = ResponseErrorCode.BadRequest)
    {
        ErrorCode = errorCode;
        State = ApiStatus.BadRequest;
        Message = message;
    }
}

public class NotFoundApiServiceResponse : ApiServiceResponse
{
    public NotFoundApiServiceResponse(string message = null, string errorCode = ResponseErrorCode.NotFound)
    {
        ErrorCode = errorCode;
        State = ApiStatus.NotFound;
        Message = message;
    }
}