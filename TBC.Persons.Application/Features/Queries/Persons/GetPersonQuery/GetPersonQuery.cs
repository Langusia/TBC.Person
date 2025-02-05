using MediatR;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Application.Features.Queries.Persons.GetPersonQuery;

public record GetPersonQuery(long Id) : IRequest<Result<Person?>>;

public class GetPersonQueryHandler(IPersonsRepository db) : IRequestHandler<GetPersonQuery, Result<Person?>>
{
    public async Task<Result<Person?>> Handle(GetPersonQuery request, CancellationToken cancellationToken)
    {
        return Result.Success(await db.GetPersonFullDataAsync(request.Id, cancellationToken: cancellationToken));
    }
}