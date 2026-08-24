using Microsoft.EntityFrameworkCore;
using MyHub.Entities;
using System.Runtime.CompilerServices;

namespace MyHub.Repository
{
    public interface IRepository<TEntity, TKey> 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        Task<TEntity?> CreateAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(TKey key);
        Task<TEntity?> UpdateAsync(TEntity entity);
        Task<TEntity?> DeleteAsync(TEntity entity);
    }
}
