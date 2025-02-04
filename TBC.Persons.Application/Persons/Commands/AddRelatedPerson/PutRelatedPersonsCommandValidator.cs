using FluentValidation;
using Microsoft.Extensions.Localization;

namespace TBC.Persons.Application.Persons.Commands.AddRelatedPerson;

public class PutRelatedPersonsCommandValidator : AbstractValidator<PutRelatedPersonCommand>
{
    public PutRelatedPersonsCommandValidator(IStringLocalizer<PutRelatedPersonCommand> localizer)
    {
        RuleFor(x => x.Id).NotNull().WithMessage(localizer["id null"]);
        RuleFor(x => x.Id).NotEmpty().WithMessage(localizer["id null"]);
        RuleFor(x => x.RelatedPersonId).NotNull().WithMessage(localizer["relatedId null"]);
        RuleFor(x => x.RelatedPersonId).NotEmpty().WithMessage(localizer["relatedId null"]);
        RuleFor(x => x.RelationType)
            .IsInEnum().WithMessage(localizer["not valid relation"]);
    }
}