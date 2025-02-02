using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Application.Persons.Commands.AddRelatedPerson;

public record PutRelatedPerson(long Id, RelationType RelationType);