namespace TBC.Persons.Domain.Messages;

public class ApiServiceResponse<T> : ApiServiceResponse
{
    public ApiServiceResponse()
    {
    }

    public ApiServiceResponse(T data, ApiServiceResponse response)
    {
        base.ErrorCode = response.ErrorCode;
        base.DetailsMessage = response.DetailsMessage;
        base.ExternalState = response.ExternalState;
        base.Message = response.Message;
        base.State = response.State;
        base.ValidationErrors = response.ValidationErrors;
        Data = data;
    }

    public T Data { get; protected set; }
}

public class SuccessApiServiceResponse<T> : ApiServiceResponse<T>
{
    public SuccessApiServiceResponse(T data, string message = null)
    {
        Data = data;
        State = ApiStatus.Ok;
        Message = message;
    }
}

public class ValidationFailedApiGenericServiceResponse<T> : ApiServiceResponse<T>
{
    public ValidationFailedApiGenericServiceResponse(string param, string errorCode = ResponseErrorCode.BadRequest)
    {
        ErrorCode = errorCode;
        Message = $"Invalid parameter '{param}'";
        State = ApiStatus.BadRequest;
    }
}

public class ExternalServiceFailedApiGenericServiceResponse<T> : ApiServiceResponse<T>
{
    public ExternalServiceFailedApiGenericServiceResponse(string serviceName, string message, string status,
        ExternalApiStatus errorStatus, string errorCode = ResponseErrorCode.ServiceCallError)
    {
        Message = message;
        DetailsMessage = $"External service '{serviceName}' failed: {status} message: {message}";
        State = ApiStatus.Failed;
        ExternalState = errorStatus;
        ErrorCode = errorCode;
    }
}

public class BadRequestApiServiceResponse<T> : ApiServiceResponse<T>
{
    public BadRequestApiServiceResponse(T data, string message = null, string errorCode = ResponseErrorCode.BadRequest,
        List<string> validationErrors = null)
    {
        ErrorCode = errorCode;
        State = ApiStatus.BadRequest;
        Message = message;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public BadRequestApiServiceResponse(string message = null, string errorCode = ResponseErrorCode.BadRequest,
        List<string> validationErrors = null)
    {
        ErrorCode = errorCode;
        State = ApiStatus.BadRequest;
        Message = message;
        ValidationErrors = validationErrors;
    }
}

public class NotFoundApiServiceResponse<T> : ApiServiceResponse<T>
{
    public NotFoundApiServiceResponse(string message = null, string errorCode = ResponseErrorCode.NotFound)
    {
        ErrorCode = errorCode;
        State = ApiStatus.NotFound;
        Message = message;
    }
}