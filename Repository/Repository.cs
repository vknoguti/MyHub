using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyHub.Data;
using MyHub.Entities;

namespace MyHub.Repository
{
    public class Repository<TEntity, TKey> where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        private ApplicationDbContext _context;
        private DbSet<TEntity> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public async Task<TEntity?> CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            var created = await _context.SaveChangesAsync();
            return created > 0 ? entity : null;
        }

        public async Task<TEntity?> GetByIdAsync(TKey key)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.Id.Equals(key));
        }

        public TEntity? GetById(TKey key)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(t => t.Id.Equals(key));
        }
        
        public async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            var idEntity = entity.Id;
            var toUpdate = await _dbSet.FirstOrDefaultAsync(t => t.Id.Equals(idEntity));

            if (toUpdate is null) return null;

            _dbSet.Entry(toUpdate).CurrentValues.SetValues(entity);

            var created = await _context.SaveChangesAsync();
            return created > 0 ? entity : null;
        }

        public async Task<TEntity?> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0 ? entity : null;
        }
    }
}
