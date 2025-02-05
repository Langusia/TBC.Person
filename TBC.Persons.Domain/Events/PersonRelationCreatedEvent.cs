using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Events;

public record PersonRelationCreatedEvent(long PersonId, RelationType RelationType, long RelatedPersonId) : BaseEvent;