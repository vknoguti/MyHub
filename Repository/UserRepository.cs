using MyHub.Data;
using MyHub.Entities;

namespace MyHub.Repository
{
    public class UserRepository<TEntity, TKey> : Repository<TEntity, TKey>, IUserRepository<TEntity, TKey>
        where TEntity : User<TKey>, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }
    }
}
