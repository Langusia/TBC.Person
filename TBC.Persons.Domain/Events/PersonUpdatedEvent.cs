using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Events;

public record PersonUpdatedEvent(
    long Id,
    string? FirstName,
    string? LastName,
    string? FirstNameEng,
    string? LastNameEng,
    Gender? Gender,
    string? PersonalNumber,
    DateTime? DateOfBirth,
    int? CityId,
    List<PhoneNumber>? PhoneNumbers
) : BaseEvent;