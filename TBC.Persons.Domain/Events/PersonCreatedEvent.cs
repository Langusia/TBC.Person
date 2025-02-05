using MediatR;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Events;

public record PersonCreatedEvent(
    string FirstName,
    string LastName,
    string? FirstNameEng,
    string? LastNameEng,
    Gender Gender,
    string PersonalNumber,
    DateTime DateOfBirth,
    int CityId,
    List<PhoneNumber> PhoneNumbers,
    string? PicturePath) : BaseEvent;

public class PersonCreatedEventHandler : INotificationHandler<PersonCreatedEvent>
{
    public Task Handle(PersonCreatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}