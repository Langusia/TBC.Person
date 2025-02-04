using TBC.Persons.Domain.Interfaces;
using TBC.Persons.Infrastructure.Database.Contexts;

namespace TBC.Persons.Infrastructure.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext, IPersonsRepository personsRepository,
        ICityRepository cityRepository)
    {
        _dbContext = dbContext;
        PersonsRepository = personsRepository;
        CityRepository = cityRepository;
    }

    public IPersonsRepository PersonsRepository { get; }
    public ICityRepository CityRepository { get; }

    public async Task<int> CompleteWorkAsync(CancellationToken token)
    {
        return await _dbContext.SaveChangesAsync(token);
    }
}