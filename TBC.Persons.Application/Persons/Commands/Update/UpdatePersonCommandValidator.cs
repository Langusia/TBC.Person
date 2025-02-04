using FluentValidation;
using Microsoft.Extensions.Localization;

namespace TBC.Persons.Application.Persons.Commands.Update;

public class UpdatePersonCommandValidator
    : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator(IStringLocalizer<UpdatePersonCommand> localizer)
    {
        RuleFor(x => x.Id).NotNull();

        RuleFor(x => x.FirstName)
            .Length(2, 50).WithMessage(localizer["FirstNameLength"])
            .Matches("^[ა-ჰ]+$").WithMessage(localizer["FirstNameGeorgianOnly"]);

        RuleFor(x => x.LastName)
            .Length(2, 50).WithMessage(localizer["LastNameLength"])
            .Matches("^[ა-ჰ]+$").WithMessage(localizer["LastNameGeorgianOnly"]);

        RuleFor(x => x.FirstNameEng)
            .Matches("^[a-zA-Z]*$").When(x => !string.IsNullOrEmpty(x.FirstNameEng))
            .WithMessage(localizer["FirstNameEngLatinOnly"]);

        RuleFor(x => x.LastNameEng)
            .Matches("^[a-zA-Z]*$").When(x => !string.IsNullOrEmpty(x.LastNameEng))
            .WithMessage(localizer["LastNameEngLatinOnly"]);


        RuleFor(x => x.PersonalNumber)
            .Matches("^\\d{11}$").WithMessage(localizer["PersonalNumberFormat"]);

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage(localizer["OnlyTwo"]);

        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage(localizer["CityIdRequired"]);

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage(localizer["DateOfBirthFuture"])
            .GreaterThan(DateTime.Now.AddYears(-99)).WithMessage(localizer["DateOfBirthOld"]);
    }
}