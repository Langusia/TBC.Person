using FluentValidation;

namespace TBC.Persons.Application.Features.Queries.Persons.GetPersonQuery;

public class GetPersonQueryValidator : AbstractValidator<GetPersonQuery>
{
    public GetPersonQueryValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}