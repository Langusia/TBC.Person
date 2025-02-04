using FluentValidation;
using Microsoft.Extensions.Localization;
using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Application.Persons.Commands.Create;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator(IStringLocalizer<CreatePersonCommand> localizer)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(localizer["FirstNameRequired"])
            .Length(2, 50).WithMessage(localizer["FirstNameLength"])
            .Matches("^[ა-ჰ]+$").WithMessage(localizer["FirstNameGeorgianOnly"]);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(localizer["LastNameRequired"])
            .Length(2, 50).WithMessage(localizer["LastNameLength"])
            .Matches("^[ა-ჰ]+$").WithMessage(localizer["LastNameGeorgianOnly"]);

        RuleFor(x => x.FirstNameEng)
            .Matches("^[a-zA-Z]*$").When(x => !string.IsNullOrEmpty(x.FirstNameEng))
            .WithMessage(localizer["FirstNameEngLatinOnly"]);

        RuleFor(x => x.LastNameEng)
            .Matches("^[a-zA-Z]*$").When(x => !string.IsNullOrEmpty(x.LastNameEng))
            .WithMessage(localizer["LastNameEngLatinOnly"]);


        RuleFor(x => x.PersonalNumber)
            .NotEmpty().WithMessage(localizer["PersonalNumberRequired"])
            .Matches("^\\d{11}$").WithMessage(localizer["PersonalNumberFormat"]);

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage(localizer["GenderRequired"]);

        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage(localizer["CityIdRequired"]);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage(localizer["DateOfBirthRequired"])
            .LessThan(DateTime.Now).WithMessage(localizer["DateOfBirthFuture"])
            .GreaterThan(DateTime.Now.AddYears(-99)).WithMessage(localizer["DateOfBirthOld"]);
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