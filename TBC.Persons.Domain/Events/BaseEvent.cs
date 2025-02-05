using MediatR;

namespace TBC.Persons.Domain.Events;

public record BaseEvent() : INotification;