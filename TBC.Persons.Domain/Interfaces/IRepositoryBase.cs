using TBC.Persons.Domain.Entities;

namespace TBC.Persons.Domain.Interfaces;

public interface IRepositoryBase<TEntity, TEntityId> where TEntity : class, IEntityBase<TEntityId>
{
    Task<bool> AnyByIdAsync(TEntityId id, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllAsync(bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(
        TEntityId id,
        bool asNoTracking = true,
        bool ensureNotNull = true,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetByIdsAsync(
        List<TEntityId> ids,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default);

    Task<TEntityId> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, bool beginTracking = false,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntityId id, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool beginTracking = false,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListByIdsAsync(List<TEntityId> ids,
        CancellationToken cancellationToken = default);
}