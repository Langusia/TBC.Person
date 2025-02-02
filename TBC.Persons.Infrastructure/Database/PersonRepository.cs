using Microsoft.EntityFrameworkCore;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Infrastructure.Database;

public class PersonRepository : RepositoryBase<Person, long>, IPersonsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PersonRepository(ApplicationDbContext db) : base(db)
    {
        _dbContext = db;
    }

    public async Task<Person?> GetWithRelatedPersons(long id)
    {
        return await _dbContext.Persons.Include(x => x.RelatedPersons)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}