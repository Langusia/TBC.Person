using System.ComponentModel.DataAnnotations;
using TBC.Persons.Domain.Enums;
using TBC.Persons.Domain.Values;

namespace TBC.Persons.Domain.Entities;

public class Person : IEntityBase<long>
{
    public long Id { get; protected set; }
    public MultiLanguageString FirstName { get; init; }
    public MultiLanguageString LastName { get; init; }
    public Gender Gender { get; init; }

    public string PersonalNumber { get; init; }

    [MaxLength(50)]

    public string? PicturePath { get; set; }

    public DateTime DateOfBirth { get; init; }
    public virtual long CityId { get; set; }
    public virtual City City { get; set; }
    public virtual List<PhoneNumber> PhoneNumbers { get; set; }
    public virtual List<RelatedPerson> RelatedPersons { get; init; } = new();
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsDeleted { get; set; }

    public bool IsAdult() => (DateTime.Today.Year - DateOfBirth.Year -
                              (DateTime.Today < DateOfBirth.AddYears(DateTime.Today.Year - DateOfBirth.Year)
                                  ? 1
                                  : 0)) >= 18;
}