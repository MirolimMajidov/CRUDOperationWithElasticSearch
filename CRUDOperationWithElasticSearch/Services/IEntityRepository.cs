using MyUser.Models;

namespace MyUser.Services;

public interface IEntityRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(int? from = null, int? size = null);
    Task<TEntity> GetByIdAsync(Guid id);
    Task<TEntity> CreateAsync(TEntity item);
    Task<TEntity> UpdateAsync(Guid id, TEntity item);
    Task<bool> DeleteAsync(Guid id);
}