using TBC.Persons.Domain.Values;

namespace TBC.Persons.Domain.Entities;

public class City : IEntityBase<long>
{
    public long Id { get; protected set; }

    public MultiLanguageString Name { get; set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsDeleted { get; private set; }
}