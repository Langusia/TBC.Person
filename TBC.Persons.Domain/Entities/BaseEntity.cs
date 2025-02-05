using System.ComponentModel.DataAnnotations.Schema;
using TBC.Persons.Domain.Events;

namespace TBC.Persons.Domain.Entities;

public abstract class BaseEntity<T> : IEntityBase<T>
{
    public T Id { get; }
    public DateTime CreatedAt { get; }
    public bool IsActive { get; }
    public bool IsDeleted { get; }

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}