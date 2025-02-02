﻿using Microsoft.EntityFrameworkCore;
using TBC.Persons.Domain.Entities;
using TBC.Persons.Domain.Interfaces;

namespace TBC.Persons.Infrastructure.Database;

public class RepositoryBase<TEntity, TEntityId> : IRepositoryBase<TEntity, TEntityId>
    where TEntity : class, IEntityBase<TEntityId>
{
    private readonly ApplicationDbContext db;

    public RepositoryBase(ApplicationDbContext db)
    {
        this.db = db;
    }

    public virtual async Task<bool> AnyByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = db.Set<TEntity>();
        return await query.AnyAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetAllAsync(bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = db.Set<TEntity>();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> GetByIdAsync(
        TEntityId id,
        bool asNoTracking = true,
        bool ensureNotNull = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = db.Set<TEntity>();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        TEntity entity = await query.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);

        return entity;
    }

    public virtual async Task<List<TEntity>> GetByIdsAsync(
        List<TEntityId> ids,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = db.Set<TEntity>();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        List<TEntity> entities = await query.Where(entity => ids.Contains(entity.Id)).ToListAsync(cancellationToken);

        return entities;
    }

    public virtual async Task<TEntityId> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await db.Set<TEntity>().AddAsync(entity, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public virtual async Task UpdateAsync(TEntity entity, bool beginTracking = false,
        CancellationToken cancellationToken = default)
    {
        // beginTracking should be true only in a "Disconnected Scenario":
        // https://www.learnentityframeworkcore.com/dbcontext/modifying-data#disconnected-scenario
        if (beginTracking)
        {
            db.Set<TEntity>().Update(entity);
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        TEntity entity = await db.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        db.Remove(entity!);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        db.Remove(entity!);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        db.Set<TEntity>().RemoveRange(entities);
        await db.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        await db.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool beginTracking = false,
        CancellationToken cancellationToken = default)
    {
        if (beginTracking)
        {
            db.Set<TEntity>().UpdateRange(entities);
        }

        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetListByIdsAsync(List<TEntityId> ids,
        CancellationToken cancellationToken = default)
    {
        return await db.Set<TEntity>()
            .Where(entity => ids.Contains(entity.Id))
            .ToListAsync(cancellationToken);
    }
}