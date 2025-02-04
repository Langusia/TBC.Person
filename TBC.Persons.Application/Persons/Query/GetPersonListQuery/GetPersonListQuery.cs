using MediatR;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Persons.Query.GetPersonListQuery;

public record GetPersonListQuery(
    string? FirstName,
    string? LastName,
    string? PersonalNumber,
    int pageSize,
    int pageIndex) : IRequest<Result<PaginatedList<Person>>>;

public class GetPersonListQueryHandler(IPersonsRepository repository)
    : IRequestHandler<GetPersonListQuery, Result<PaginatedList<Person>>>
{
    public async Task<Result<PaginatedList<Person>>> Handle(GetPersonListQuery request,
        CancellationToken cancellationToken)
    {
        var persons = await repository.GetPersonsAsync(
            request.FirstName, request.LastName, request.PersonalNumber, request.pageIndex, request.pageSize,
            cancellationToken);
        return persons;
    }
}
//სახელი, გვარი, პირადი ნომრის მიხედვით