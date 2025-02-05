using Microsoft.EntityFrameworkCore;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;
using TBC.Persons.Domain.Queries;
using TBC.Persons.Domain.Values;
using TBC.Persons.Infrastructure.Db.Contexts;

namespace TBC.Persons.Infrastructure.Db.Repositories;

public class PersonRepository : RepositoryBase<Person, long>, IPersonsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PersonRepository(ApplicationDbContext db) : base(db)
    {
        _dbContext = db;
    }

    public async Task<Person?> GetPersonFullDataAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbContext.Persons
            .Include(x => x.RelatedPersons)
            .Include(x => x.PhoneNumbers)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Person?> GetByPersonalNumberAsync(string personalNumber, CancellationToken cancellationToken)
    {
        return await _dbContext.Persons.FirstOrDefaultAsync(x => x.PersonalNumber == personalNumber,
            cancellationToken: cancellationToken);
    }

    public async Task<List<RelationReportItem>> GetReportAsync(
        CancellationToken cancellationToken)
    {
        var report = await _dbContext.RelatedPersons
            .GroupBy(pr => new { pr.RelationType, pr.PersonId }) // ✅ Grouping by IDs
            .Select(g => new
            {
                g.Key.RelationType,
                g.Key.PersonId,
                RelationCount = g.Count()
            })
            .Join(_dbContext.Persons, // ✅ Joining with Persons after grouping
                grouped => grouped.PersonId,
                person => person.Id,
                (grouped, person) => new RelationReportItem
                {
                    FullNameGeorgian = person.FirstName.Georgian + " " + person.LastName.Georgian,
                    FullNameEnglish = person.FirstName.English + " " + person.LastName.English,
                    RelationType = grouped.RelationType,
                    RelationCount = grouped.RelationCount
                })
            .OrderByDescending(r => r.RelationCount) // Optional: Sorting
            .ToListAsync(cancellationToken);

        return report;
    }

    public async Task<PaginatedList<Person>?> GetPersonsAsync(string? firstName, string? lastName,
        string? personalNumber,
        int pageNumber,
        int pageSize, CancellationToken cancellationToken)
    {
        var query = _dbContext.Persons.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(firstName))
            query = query.Where(p => EF.Functions.Like(p.FirstName.Georgian, $"%{firstName}%") ||
                                     EF.Functions.Like(p.FirstName.English, $"%{firstName}%"));

        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(p => EF.Functions.Like(p.LastName.Georgian, $"%{lastName}%") ||
                                     EF.Functions.Like(p.LastName.English, $"%{lastName}%"));

        if (!string.IsNullOrWhiteSpace(personalNumber))
            query = query.Where(p => EF.Functions.Like(p.PersonalNumber, $"%{personalNumber}%"));

        // Execute queries separately
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<Person>(items, totalCount, pageNumber, pageSize);
    }
}