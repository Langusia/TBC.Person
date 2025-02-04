using Microsoft.EntityFrameworkCore;
using TBC.Persons.Domain;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;
using TBC.Persons.Infrastructure.Database.Contexts;

namespace TBC.Persons.Infrastructure.Database;

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