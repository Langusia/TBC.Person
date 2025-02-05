using System.ComponentModel.DataAnnotations;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Entities;

public class RelatedPerson : BaseEntity<long>
{
    public long Id { get; protected set; }


    [Required]
    [EnumDataType(typeof(RelationType))]
    public RelationType RelationType { get; set; }

    public long PersonId { get; init; }
    public long RelatedPersonId { get; init; }

    public virtual Person Person { get; init; }
    public virtual Person LinkedPerson { get; init; }
}