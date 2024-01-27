using MyUser.Models;

namespace MyUser.Services;

public interface IEntityRepository<TEntity> where TEntity : BaseEntity
{
    IEnumerable<TEntity> GetAll();
    TEntity GetById(Guid id);
    void Create(TEntity item);
    void Remove(TEntity item);
    void Update(TEntity item);
}