using System.ComponentModel.DataAnnotations;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Enums;

public class Person : IEntityBase<long>
{
    public long Id { get; set; }

    [Required, MinLength(2), MaxLength(50)]
    [RegularExpression("^[ა-ჰ]+$|^[A-Za-z]+$", ErrorMessage = "Name must contain only Georgian or only Latin letters.")]
    public string FirstName { get; init; }

    [Required, MinLength(2), MaxLength(50)]
    [RegularExpression("^[ა-ჰ]+$|^[A-Za-z]+$",
        ErrorMessage = "Surname must contain only Georgian or only Latin letters.")]
    public string LastName { get; init; }

    [Required]
    public Gender Gender { get; init; }

    [Required, MinLength(2), MaxLength(50)]
    [RegularExpression("^\\d{11}$", ErrorMessage = "Personal number must be exactly 11 digits.")]
    public string PersonalNumber { get; init; }

    [Required]
    public DateTime DateOfBirth { get; init; }

    public int CityId { get; init; }

    public List<PhoneNumber> PhoneNumbers { get; set; }

    public string? PicturePath { get; init; }

    public List<RelatedPerson> RelatedPersons { get; init; } = new();

    public bool IsAdult() => (DateTime.Today.Year - DateOfBirth.Year -
                              (DateTime.Today < DateOfBirth.AddYears(DateTime.Today.Year - DateOfBirth.Year)
                                  ? 1
                                  : 0)) >= 18;
}