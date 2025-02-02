using System.ComponentModel.DataAnnotations;
using TBC.Persons.Domain.Enums;

namespace TBC.Persons.Domain.Entities;

public record PhoneNumber
{
    public long Id { get; init; }
    [Required]
    [EnumDataType(typeof(PhoneType))]
    public PhoneType Type { get; init; }

    [Required, MinLength(4), MaxLength(50)]
    public string Number { get; init; }
}