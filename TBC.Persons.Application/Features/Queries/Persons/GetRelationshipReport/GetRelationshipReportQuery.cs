using System.Diagnostics.CodeAnalysis;
using MediatR;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Interfaces;
using TBC.Persons.Domain.Queries;

namespace TBC.Persons.Application.Features.Queries.Persons.GetRelationshipReport;

[ExcludeFromCodeCoverage]
public record GetRelationshipReportQuery : IRequest<Result<List<RelationReportItem>>>;

public class
    GetRelationshipReportQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetRelationshipReportQuery, Result<List<RelationReportItem>>>
{
    public async Task<Result<List<RelationReportItem>>> Handle(GetRelationshipReportQuery request,
        CancellationToken cancellationToken)
    {
        var report = await uow.PersonsRepository.GetReportAsync(cancellationToken);
        return Result.Success(report);
    }
}