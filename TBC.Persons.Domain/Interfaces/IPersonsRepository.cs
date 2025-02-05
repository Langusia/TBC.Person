using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Queries;
using TBC.Persons.Domain.Values;

namespace TBC.Persons.Domain.Interfaces;

public interface IPersonsRepository : IRepositoryBase<Person, long>
{
    Task<Person?> GetPersonFullDataAsync(long id, CancellationToken cancellationToken);
    Task<Person?> GetByPersonalNumberAsync(string personalNumber, CancellationToken cancellationToken);
    Task<List<RelationReportItem>> GetReportAsync(CancellationToken cancellationToken);

    Task<PaginatedList<Person>?> GetPersonsAsync(string? firstName, string? lastName, string? personalNumber,
        int pageNumber,
        int pageSize, CancellationToken cancellationToken);
}