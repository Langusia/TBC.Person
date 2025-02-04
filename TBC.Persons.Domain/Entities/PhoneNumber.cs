using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Entities;

public record PhoneNumber : IEntityBase<long>
{
    public long Id { get; protected set; }


    [Required]
    [EnumDataType(typeof(PhoneType))]
    public PhoneType Type { get; init; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Number { get; init; }

    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }= true;
    public bool IsDeleted { get; private set; }
}