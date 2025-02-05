using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Events;

public record PersonRelationUpdatedEvent(
    long Id,
    RelationType RelationType
) : BaseEvent;