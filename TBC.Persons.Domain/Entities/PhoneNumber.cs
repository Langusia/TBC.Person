using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Entities;

public class PhoneNumber : BaseEntity<long>
{
    public long Id { get; set; }

    [Required]
    [EnumDataType(typeof(PhoneType))]
    public PhoneType Type { get; init; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Number { get; init; }
}