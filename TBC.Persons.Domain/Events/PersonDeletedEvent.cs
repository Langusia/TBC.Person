namespace TBC.Persons.Domain.Events;

public record PersonDeletedEvent(
    long Id
) : BaseEvent;