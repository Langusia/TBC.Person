﻿using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;
using TBC.Persons.Infrastructure.Database.Contexts;

namespace TBC.Persons.Infrastructure.Database;

public class CityRepository : RepositoryBase<City, long>, ICityRepository
{
    public CityRepository(ApplicationDbContext db) : base(db)
    {
    }
}