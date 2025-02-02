using FluentValidation;
using TBC.Persons.Application.Persons.Commands.Create;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Application.Persons.Commands;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(50)
            .MinimumLength(2);
        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .MinimumLength(2);
        RuleFor(x => x.PersonalNumber)
            .Matches("^\\d{11}$")
            .NotNull();
        RuleFor(x => x.Gender)
            .NotNull();
        RuleFor(x => x.CityId)
            .NotNull();
        RuleFor(x => x.PicturePath)
            .NotNull().NotEmpty();
        RuleFor(x => x.DateOfBirth)
            .Must(x => x > DateTime.Now.AddYears(-99))
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.PhoneNumbers)
            .SetValidator(new PhoneNumberValidator());
    }
}

public class PhoneNumberValidator : AbstractValidator<PhoneNumber>
{
    public PhoneNumberValidator()
    {
        RuleFor(x => x.Number)
            .MaximumLength(50)
            .MinimumLength(2);
        RuleFor(x => x.Type)
            .NotNull();
    }
}