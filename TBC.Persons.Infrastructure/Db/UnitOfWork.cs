using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TBC.Persons.Domain.Interfaces;
using TBC.Persons.Infrastructure.Db.Contexts;

namespace TBC.Persons.Infrastructure.Db;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction _currentTransaction;

    public UnitOfWork(ApplicationDbContext dbContext, IPersonsRepository personsRepository,
        ICityRepository cityRepository)
    {
        _dbContext = dbContext;
        PersonsRepository = personsRepository;
        CityRepository = cityRepository;
    }

    public IPersonsRepository PersonsRepository { get; }
    public ICityRepository CityRepository { get; }


    public IDbConnection GetDbConnection()
    {
        return _dbContext.Database.GetDbConnection();
    }

    public async Task<int> CompleteWorkAsync(CancellationToken token)
    {
        return await _dbContext.SaveChangesAsync(token);
    }
}