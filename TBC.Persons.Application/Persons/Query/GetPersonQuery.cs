using MediatR;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Persons.Query;

public record GetPersonQuery(long Id) : IRequest<Person?>;

public class GetPersonQueryHandler(IPersonsRepository db) : IRequestHandler<GetPersonQuery, Person?>
{
    public async Task<Person?> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        return await db.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
    }
}