namespace TBC.Persons.Domain.Entities;

public interface IEntityBase<T>
{
    T Id { get; set; }
}