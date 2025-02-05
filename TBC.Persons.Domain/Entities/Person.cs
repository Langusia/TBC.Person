using System.ComponentModel.DataAnnotations;
using TBC.Persons.Domain.Enums;
using TBC.Persons.Domain.Values;

namespace TBC.Persons.Domain.Entities;

public class Person : BaseEntity<long>
{
    public long Id { get;  set; }
    public MultiLanguageString FirstName { get; set; } = new(null, null);
    public MultiLanguageString LastName { get; set; } = new(null, null);
    public Gender Gender { get; init; }

    public string PersonalNumber { get; init; }

    [MaxLength(50)]

    public string? PicturePath { get; set; }

    public DateTime DateOfBirth { get; init; }
    public virtual long CityId { get; set; }
    public virtual City City { get; set; }
    public virtual List<PhoneNumber> PhoneNumbers { get; set; }
    public virtual List<RelatedPerson> RelatedPersons { get; set; } = new();
}