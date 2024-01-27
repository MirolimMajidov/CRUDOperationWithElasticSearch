using Microsoft.EntityFrameworkCore;
using MyUser.Models;

namespace MyUser.Services;

public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
{
    readonly UserContext _context;
    readonly DbSet<TEntity> _dbSet;

    public EntityRepository(UserContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _dbSet.AsNoTracking();
    }

    public TEntity GetById(Guid id)
    {
        return _dbSet.AsNoTracking().FirstOrDefault(i => i.Id == id);
    }

    public void Create(TEntity item)
    {
        item.Id = Guid.NewGuid();
        _dbSet.Add(item);
        _context.SaveChanges();
    }

    public void Update(TEntity item)
    {
        _context.Entry(item).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Remove(TEntity item)
    {
        _dbSet.Remove(item);
        _context.SaveChanges();
    }
}