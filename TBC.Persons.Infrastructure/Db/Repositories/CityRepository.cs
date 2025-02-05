using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;
using TBC.Persons.Infrastructure.Db.Contexts;

namespace TBC.Persons.Infrastructure.Db.Repositories;

public class CityRepository : RepositoryBase<City, long>, ICityRepository
{
    public CityRepository(ApplicationDbContext db) : base(db)
    {
    }
}