namespace TBC.Persons.Domain.Messages;

public enum ApiStatus
{
    Ok,
    NotFound,
    Failed,
    BadRequest,
    AlreadyExists
}

public enum ExternalApiStatus
{
    Ok,
    NotFound,
    Failed
}