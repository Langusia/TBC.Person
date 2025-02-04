using FluentValidation;

namespace TBC.Persons.Application.Persons.Query.GetPersonQuery;

public class GetPersonQueryValidator : AbstractValidator<GetPersonQuery>
{
    public GetPersonQueryValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}