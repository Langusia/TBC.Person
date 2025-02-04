using System.ComponentModel.DataAnnotations;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Entities;

public record RelatedPerson : IEntityBase<long>
{
    public long Id { get; protected set; }


    [Required]
    [EnumDataType(typeof(RelationType))]
    public RelationType RelationType { get; set; }

    public long PersonId { get; init; }
    public long RelatedPersonId { get; init; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
    public Person Person { get; init; }
    public Person LinkedPerson { get; init; }
}