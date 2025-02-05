namespace TBC.Persons.Domain.Entities;

public interface IEntityBase<T>
{
    T Id { get; }
    public DateTime CreatedAt { get; }

    public bool IsActive { get; }

    public bool IsDeleted { get; }
}