using FluentValidation;
using Microsoft.Extensions.Localization;

namespace TBC.Persons.Application.Features.Queries.Persons.GetPersonListQuery;

public class GetPersonListQueryValidator : AbstractValidator<GetPersonListQuery>
{
    public GetPersonListQueryValidator(IStringLocalizer<GetPersonListQuery> localizer)
    {
        RuleFor(x => x.pageSize)
            .Must(x => x > 0)
            .WithMessage(localizer["pageSizeMustMoreThenZero"].Value)
            .NotNull().WithMessage(localizer["pageSizeNull"].Value);

        RuleFor(x => x.pageIndex)
            .Must(x => x > 0).WithMessage(localizer["pageIndexMustMoreThenZero"].Value)
            .NotNull().WithMessage(localizer["pageIndexNull"].Value);
    }
}